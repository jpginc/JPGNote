using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    [KnownType(typeof(UserAction))]
    internal class UserActionManager : Manager, IManagerAndActionProvider
    {
        [IgnoreDataMember] public static UserActionManager Instance { get; set; }
        [IgnoreDataMember] public override string ManageText => "Manage Actions";
        [IgnoreDataMember] public override string CreateChoiceText => "Create New Action";
        [IgnoreDataMember] public override string DeleteChoiceText => "Delete Actions";

        public void ManageUserActions()
        {
            JpgActionManager.PushActionContext(new UserActionManagerMenu());
        }

        public void ExportActions(UserActionResult obj)
        {
            CancellableObj<IEnumerable<ITreeViewChoice>> choices = GuiManager.Instance.GetChoicesBlocking(GetUserActionChoices(), "Select commands to export");
            if (choices.ResponseType != UserActionResult.ResultType.Canceled && choices.Result.Count() != 0) {
                var file = GuiManager.Instance.SaveFile("Select file to save to");
                if(file.ResponseType != UserActionResult.ResultType.Canceled && ! file.Result.Equals("")) 
                {
                    var fileName = file.Result;
                    var toExport = choices.Result.Select(c => (c as AutoAction).Creatable as UserAction).ToList();
                    var stream1 = new MemoryStream();
                    var ser = new DataContractJsonSerializer(toExport.GetType());
                    ser.WriteObject(stream1, toExport);
                    stream1.Position = 0;
                    var sr = new StreamReader(stream1);
                    var writer = new StreamWriter(fileName);
                    // Rewrite the entire value of s to the file
                    stream1.Position = 0;
                    writer.Write(sr.ReadToEnd());
                    writer.Close();
                }
            }

            JpgActionManager.UnrollActionContext();
        }

        public void ImportActions(UserActionResult obj)
        {
            var fileChoice = GuiManager.Instance.SelectFile("Select file to import");
            if(fileChoice.ResponseType != UserActionResult.ResultType.Canceled && ! fileChoice.Result.Equals("")) 
            {
                var FileName = fileChoice.Result;
                StreamReader file = null;
                try
                {
                    file = File.OpenText(FileName);
                    var s = file.ReadToEnd();
                    file.Close();
                    var ms = new MemoryStream(Encoding.UTF8.GetBytes(s));
                    var ser = new DataContractJsonSerializer(typeof(List<UserAction>));
                    var actions = ser.ReadObject(ms) as List<UserAction>;
                    ms.Close();
                    if(actions.Any(a => Creatables.Any(c => (c as UserAction).Name.Equals(a.Name))))
                    {
                        if(UserNotifier.Confirm("Some actions have the same name. Skip importing those actions?", GuiManager.Instance.GetActiveGui()))
                        {
                            actions = actions.Where(a => ! Creatables.Any(c => (c as UserAction).Name.Equals(a.Name))).ToList();
                        }
                    }
                    actions.ForEach(a => {
                        a.UniqueId = Guid.NewGuid().ToString("N");
                        Creatables.Add(a);
                        });
                    Save();
                }
                catch (Exception e)
                {
                    UserNotifier.Error("Error importing commands\r\n" + e.StackTrace, GuiManager.Instance.GetActiveGui());
                    Console.WriteLine(e.StackTrace);
                    //throw e;
                }
                finally
                {
                    file?.Close();
                }
            }
            JpgActionManager.UnrollActionContext();
        }

        public IEnumerable<ITreeViewChoice> GetUserActionChoices()
        {
            return Creatables.Select(u => new AutoAction(u, this));
        }

        public void CreateNewUserAction(UserActionResult obj)
        {
            var userAction = new UserAction();
            if (CreatableWizard.GetRequiredFields(userAction))
            {
                Creatables.Add(userAction);
                Save();
                JpgActionManager.PushActionContext(new AutoMenu(userAction, this));
            }
        }

        public override void New(UserActionResult obj)
        {
            CreateNewUserAction(obj);
        }

        public UserAction GetAction(string name)
        {
            foreach (var a in Creatables)
            {
                //Console.WriteLine("Here " + a.EditChoiceText + " and " + name);
                if(((UserAction) a).Name.Equals(name))
                    return (UserAction)a;
            }
            return null;
        }

        public IEnumerable<ITreeViewChoice> GetPortCommands()
        {
            return Creatables.Where(c => ((UserAction) c).Command.Contains("{{PORT}}"))
                .Select(c => new AutoAction(c, this));
        }
        public InputType InputType => InputType.Single;
        public IEnumerable<ITreeViewChoice> GetActions() 
        {
            return Creatables.Select(u => new AutoAction(u, this));
        }
        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }
    }
}