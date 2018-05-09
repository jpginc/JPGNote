using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class MainMenu : SimpleActionProvider
    {
        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            var c = new List<ITreeViewChoice>
            {
                new CreateNoteAction(),
                new ExitAction(),
                new DeleteNotesAction()
            };
            return c.Concat(NotesManager.Instance.GetNoteChoices());
        }
    }
}