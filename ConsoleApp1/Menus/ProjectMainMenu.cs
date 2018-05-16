using System;
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
            var a = new List<ITreeViewChoice>
            {
                new NewNoteChoice(),
                new OpenLoggedSshSessionChoice(_project),
                new ExitChoice(),
                new DeleteNotesAction(),
                new ImportTargetsAction(_project),
                new ChoiceToActionProvider(new SelectCommandToRunMenu(_project), "Run Command")
            };

            var b = new ManageableCreatable(_project.TargetManager).GetActions();
            var c = NotesManager.Instance.GetNoteChoices();
            var d = new ManageableCreatable(_project.PortManager).GetActions();
            return a.Concat(b.Concat(c.Concat(d)));
        }
    }
}