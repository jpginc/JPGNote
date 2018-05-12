using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class PortManager : Manager, IManagerAndActionProvider 
    {
        [IgnoreDataMember] public static PortManager Instance { get; set; }
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
            return Creatables.Select(c => new AutoAction(c, this));
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }

        public void AddPremade(Port port)
        {
            var existing = Creatables.FirstOrDefault(p => ((Port)p).Equals(port));
            if (existing == null)
            {
                Creatables.Add(port);
            }
            else
            {
                ((Port) existing).Notes += "\n" + port.Notes;
            }
        }
    }
}