using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class ProjectMainMenu : SimpleActionProvider
    {
        private readonly Project _project;

        public ProjectMainMenu(Project project)
        {
            _project = project;
        }

        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            var c = new List<ITreeViewChoice>
            {
                new NewNoteChoice(),
                new OpenLoggedSshSessionChoice(_project),
                new ExitChoice(),
                new DeleteNotesAction()
            };

            //todo this is supposed to be createing a "manage scope" menu
            IEnumerable<ITreeViewChoice> a = new ManageableCreatable(new TargetManager(ProjectSettingsClass.Instance)).GetActions();

            return c.Concat(a.Concat(NotesManager.Instance.GetNoteChoices()));
        }
    }
}