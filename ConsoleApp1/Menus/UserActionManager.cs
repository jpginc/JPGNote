using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class UserActionManager : ManagerAndActionProvider
    {
        [IgnoreDataMember] public static UserActionManager Instance { get; set; }
        [IgnoreDataMember] public override string ManageText => "Manage Actions";
        [IgnoreDataMember] public override string CreateChoiceText => "New Action";
        [IgnoreDataMember] public override string DeleteChoiceText => "Delete Actions";

        public IEnumerable<ITreeViewChoice> GetUserActionChoices()
        {
            return Creatables.Select(u => new AutoAction(u, this));
        }

        public override void New(UserActionResult obj)
        {
            var userAction = new UserAction();
            if (CreatableWizard.GetRequiredFields(userAction))
            {
                Creatables.Add(userAction);
                Save();
                JpgActionManager.PushActionContext(new AutoMenu(userAction, this));
            }
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
    }
}