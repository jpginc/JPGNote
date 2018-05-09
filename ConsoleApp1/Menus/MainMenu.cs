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
                new NewNoteChoice(),
                new ExitChoice(),
                new DeleteNotesAction()
            };
            return c.Concat(NotesManager.Instance.GetNoteChoices());
        }
    }
}