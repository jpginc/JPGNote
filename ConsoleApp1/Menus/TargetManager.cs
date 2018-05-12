using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    [KnownType(typeof(Target))]
    internal class TargetManager : IManagerAndActionProvider
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
            lock (Creatables)
            {
                Creatables.Remove(creatable);
            }
            Save();
        }

        public void New(UserActionResult obj)
        {
            var target = new Target();
            if (CreatableWizard.GetRequiredFields(target))
            {
                lock (Creatables)
                {
                    Creatables.Add(target);
                    Save();
                }

            }
        }

        public InputType InputType => InputType.Multi;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return Creatables.Select(c => new AutoAction(c, this));
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }

        public void AddPremade(Target target)
        {
            if (! Creatables.Any(p => ((Target) p).IpOrDomain.Equals(target.IpOrDomain)))
            {
                Creatables.Add(target);
            }
        }
    }

    internal interface IManagerAndActionProvider : IActionProvider , IManager
    {
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