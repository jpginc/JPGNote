using System;
using System.Collections.Generic;
using System.Linq;
using GLib;

namespace ConsoleApp1
{
    internal class JpgActionManager
    {
        public static IActionContext ActionContext { get; set; }
        public static IEnumerable<ITreeViewChoice> GetActions()
        {
            if (ActionContext == null)
            {
                var c = new List<ITreeViewChoice>
                {
                    new TreeViewChoice("New Note")
                    {
                        AcceptHandler = NotesManager.Instance.NewNoteAction
                    },
                    new TreeViewChoice("Settings"),
                    new TreeViewChoice("Exit") {AcceptHandler = (a) => Environment.Exit(0)}
                };

                return c.Concat(NotesManager.Instance.GetNoteChoices());
            }
            else
            {
                Console.WriteLine("getting the actions choices");
                return ActionContext.GetChoices();
            }
        }

        public static ITreeViewChoice AcceptOnSelectAction(string text)
        {
            return new TreeViewChoice(text) {SelectHandler = MainWindow.Instance.Accept};
        }
    }
}