using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class BuiltInActionProvider
    {
        private BuiltInActionProvider()
        {
        }
        public static BuiltInActionProvider Instance { get; } = new BuiltInActionProvider();

        public IEnumerable<ITreeViewChoice> GetActions()
        {
                var c = new List<ITreeViewChoice>
                {
                    new SimpleTreeViewChoice("New Note")
                    {
                        AcceptHandler = NotesManager.Instance.NewNoteAction
                    },
                    new SimpleTreeViewChoice("Delete Notes")
                        {AcceptHandler = DeleteNotes},
                    new SimpleTreeViewChoice("Settings"),
                    new SimpleTreeViewChoice("Exit") {AcceptHandler = (a) => Environment.Exit(0)}
                };
                return c.Concat(NotesManager.Instance.GetNoteChoices());
        }

        private void DeleteNotes(UserActionResult obj)
        {
            var notesToDelete = SelectionHelper.Instance.SelectNotes("Select notes to delete");
        }
    }
}