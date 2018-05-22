using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class MachineManagerMenu : SimpleActionProvider
    {
        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            var machines = MachineManager.Instance.GetMachineChoices();
            var c = new List<ITreeViewChoice>
            {
                new NewMachineChoice()
            };
            return machines.Concat(c);
        }
    }
}