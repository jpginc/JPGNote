using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.BuiltInActions;
using GLib;

namespace ConsoleApp1
{
    public class JpgActionManager : SimpleActionProvider
    {
        public IActionProvider CurrentActionProvider => _actionProviders.Last();

        private static readonly List<IActionProvider> _actionProviders 
            = new List<IActionProvider>() {new ProgramMenu()}; 

        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            return CurrentActionProvider.GetActions();
        }

        public static void PushActionContext(IActionProvider action)
        {
            _actionProviders.Add(action);
        }

        public static void UnrollActionContext()
        {
            if (_actionProviders.Count > 1)
            {
                _actionProviders.Remove(_actionProviders.Last());
            }
        }
    }
}