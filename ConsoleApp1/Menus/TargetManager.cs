using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    [KnownType(typeof(Target))]
    public class TargetManager : IManagerAndActionProvider
    {
        [IgnoreDataMember] public ProjectPersistence Settings;

        [IgnoreDataMember] public string ManageText => "Manage Targets";
        [IgnoreDataMember] public string CreateChoiceText => "Create Target";

        [IgnoreDataMember] public string DeleteChoiceText => "Delete Targets";

        [DataMember] public List<ICreatable> Creatables { get; set; } = new List<ICreatable>();

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

        public bool HasChildren()
        {
            return true;
        }

        public IEnumerable<ICreatable> GetChildren(ICreatable parent)
        {
            if (parent is Target)
            {
                return GetChildren((Target) parent);
            }
            return Enumerable.Empty<ICreatable>();
        }

        public InputType InputType => InputType.Single;
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

        public IEnumerable<ICreatable> GetChildren(Target target)
        {
            return Settings.PortManager.GetChildren(target);
        }

        public Target GetTarget(string targetName)
        {
            return Creatables.FirstOrDefault(t => ((Target) t).IpOrDomain == targetName) as Target;
        }
    }

    internal interface IManagerAndActionProvider : IActionProvider , IManager
    {
    }

    [DataContract]
    public abstract class ManagerAndActionProvider : Manager, IManagerAndActionProvider
    {
        public InputType InputType => InputType.Multi;
        public virtual IEnumerable<ITreeViewChoice> GetActions()
        {
            return Creatables.Select(c => new AutoAction(c, this));
        }

        public virtual ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }
    }

    [DataContract]
    public class Target : ICreatable
    {
        [DataMember]
        [AutoSingleLineString]
        [Wizard]
        public string IpOrDomain { get; set; }
        [DataMember] public List<string> CommandsRun { get; set; } = new List<string>();
        [IgnoreDataMember] public string EditChoiceText => IpOrDomain;

    }
}