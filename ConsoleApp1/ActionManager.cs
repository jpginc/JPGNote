using System;
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
                new TreeViewChoice("New Note")
                {
                    AcceptHandler = NotesManager.Instance.NewNoteAction
                },
                new TreeViewChoice("Settings"),
                new TreeViewChoice("Exit") { AcceptHandler = (a) => Environment.Exit(0) }
            };

            return c.Concat(NotesManager.Instance.GetNoteChoices());
        }

        public static ITreeViewChoice AcceptOnSelectAction(string text)
        {
            return new TreeViewChoice(text) {SelectHandler = MainWindow.Instance.Accept};
        }
    }
}