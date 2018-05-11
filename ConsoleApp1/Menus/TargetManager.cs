using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    [KnownType(typeof(Target))]
    internal class TargetManager : IManager
    {
        [IgnoreDataMember] public ProjectSettingsClass Settings;

        [IgnoreDataMember] public string ManageText => "Manage Targets";
        [IgnoreDataMember] public string CreateChoiceText => "Create Target";

        [IgnoreDataMember] public string DeleteChoiceText => "Delete Targets";

        [DataMember] public List<ICreatable> Creatables { get; set; } = new List<ICreatable>();
        [IgnoreDataMember] public static TargetManager Instance { get; set; }

        public TargetManager()
        {
        }

        public void Save()
        {
            Settings.Save();
        }

        public void Delete(ICreatable creatable)
        {
            Creatables.Remove(creatable);
            Save();
        }

        public void New(UserActionResult obj)
        {
            //todo wizard 
            var target = new Target();
            if (CreatableWizard.GetRequiredFields(target))
            {
                Creatables.Add(target);
            }
            UserNotifier.Error("Not Implemented yet!");
        }
    }

    internal static class CreatableWizard
    {
        public static bool GetRequiredFields(ICreatable obj)
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                if (prop.GetCustomAttributes(typeof(Wizard), false).Any())
                {
                    Func<CancellableObj<string>> inputFunc = null;
                    var prompt = "Enter value for " + prop.Name;
                    if (prop.GetCustomAttributes(typeof(AutoSingleLineString), false).Any())
                    {
                        inputFunc = () => GuiManager.Instance.GetSingleLineInput(prompt, "");
                    }
                    else if (prop.GetCustomAttributes(typeof(AutoMultiLineString), false).Any())
                    {
                        inputFunc = () => GuiManager.Instance.GetMultiLineInput(prompt, "");
                    }

                    var result = inputFunc.Invoke();
                    if (result.ResponseType == UserActionResult.ResultType.Canceled)
                    {
                        return false;
                    }

                    prop.SetValue(obj, result.Result);
                }
            }
            return true;
        }
    }

    [DataContract]
    internal class Target : ICreatable
    {
        [DataMember]
        [AutoSingleLineString]
        [Wizard]
        public string IpOrDomain { get; set; }

        [IgnoreDataMember] public string EditChoiceText => IpOrDomain;
    }
}