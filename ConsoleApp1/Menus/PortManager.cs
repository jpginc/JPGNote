using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    public class PortManager : Manager, IManagerAndActionProvider 
    {
        [IgnoreDataMember] public override string ManageText => "Manage Ports";
        [IgnoreDataMember] public override string CreateChoiceText => "New Port";
        [IgnoreDataMember] public override string DeleteChoiceText => "Delete Ports";
        public override void New(UserActionResult obj)
        {
            var port = new Port();
            if (CreatableWizard.GetRequiredFields(port))
            {
                Creatables.Add(port);
                Save();
            }
        }

        public InputType InputType => InputType.Multi;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return Creatables.Where(c => ((IDoneable) c).ScanItemStatus != ScanItemState.Done)
                .Select(c => new AutoAction(c, this));
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }

        public void AddPremade(Port port)
        {
            var existing = Creatables.FirstOrDefault(p =>
            {
                var c = (Port)p;
                return c.Target.Equals(port.Target) && c.PortNumber.Equals(port.PortNumber);
            });
            if (existing == null)
            {
                Creatables.Add(port);
            }
            else
            {
                ((Port) existing).Notes += "\n" + port.Notes;
            }
        }

        public IEnumerable<ICreatable> GetChildren(Target target)
        {
            return Creatables.Where(c => ((Port) c).Target.Equals(target.IpOrDomain));
        }

        public Port GetPort(string portNumber)
        {
            return (Port) Creatables.FirstOrDefault(c => ((Port) c).PortNumber.Equals(portNumber));
        }
    }
}