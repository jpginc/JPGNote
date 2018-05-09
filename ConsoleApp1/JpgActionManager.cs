using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.BuiltInActions;
using GLib;

namespace ConsoleApp1
{
    internal class JpgActionManager : IActionProvider
    {
        public static JpgActionManager Instance { get; } = new JpgActionManager();

        public IActionProvider CurrentActionProvider => _actionProviders.Last();

        private readonly List<IActionProvider> _actionProviders; 

        private JpgActionManager()
        {
            _actionProviders = new List<IActionProvider>() {BuiltInActionProvider.Instance};
        }

        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return CurrentActionProvider.GetActions();
        }

        public void PushActionContext(IActionProvider action)
        {
            _actionProviders.Add(action);
        }

        public void UnrollActionContext()
        {
            if (_actionProviders.Count > 1)
            {
                _actionProviders.Remove(_actionProviders.Last());
            }
        }

        IEnumerable<ITreeViewChoice> IActionProvider.GetActions()
        {
            return GetActions();
        }
    }
}