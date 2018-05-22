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

        private static List<INote> _notes = new List<INote>();

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
            if (_notes.Count > 0)
            {
                _notes.Remove(_notes.Last());
            }
        }

        public static void PushActionContext(INote note)
        {
            _notes.Add(note);
        }

        public static INote GetNoteContext()
        {
            return _notes.FirstOrDefault();
        }
    }
}