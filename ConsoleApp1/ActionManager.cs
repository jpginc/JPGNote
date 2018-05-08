using System.Collections.Generic;
using System.Linq;
using GLib;

namespace ConsoleApp1
{
    internal class ActionManager
    {
        public static IEnumerable<ITreeViewChoice> GetActions()
        {
            var c = new List<ITreeViewChoice>
            {
                new NewNoteTreeViewChoice("New Note"),
                new SimpleTreeViewChoice("Settings"),
                new SimpleTreeViewChoice("Exit")
            };

            return c.Concat(NotesManager.Instance.GetNoteChoices());
        }
    }
}