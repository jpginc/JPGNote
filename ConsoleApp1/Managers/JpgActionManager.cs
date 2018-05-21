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

        private static readonly List<IEnumerable<INote>> Notes = new List<IEnumerable<INote>>();

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
            if (Notes.Count > 0)
            {
                Notes.Remove(Notes.Last());
            }
        }

        public static void PushActionContext(IEnumerable<INote> note)
        {
            Notes.Add(note);
        }
        public static void PushActionContext(INote note)
        {
            Notes.Add(new List<INote>() {note});
        }

        public static IEnumerable<INote> GetNoteContext()
        {
            return Notes.FirstOrDefault();
        }
    }
}