using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class SelectionHelper
    {
        private SelectionHelper()
        {
        }

        public static SelectionHelper Instance { get; } = new SelectionHelper();

        public CancellableEnumerable<Note> SelectNotes(string prompt)
        {
            UserActionResult choices = GuiManager.Instance.GetChoices(NotesManager.Instance.GetNoteChoices(), prompt);
            if (choices.Result == UserActionResult.ResultType.Accept)
            {
                return null;
            }

            return new CancellableEnumerable<Note>(Enumerable.Empty<Note>()) {IsCancelled = true};
        }
    }
}