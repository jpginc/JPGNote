using System.Collections.Generic;

namespace ConsoleApp1.BuiltInActions
{
    internal class AutoMenu : SimpleActionProvider
    {
        private readonly object _machine;

        public AutoMenu(object machine)
        {
            _machine = machine;
        }

        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            return AutoSetPropertyProvider.GetActions(_machine);
        }
    }
}