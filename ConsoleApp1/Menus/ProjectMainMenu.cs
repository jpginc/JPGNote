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
            var c = new List<ITreeViewChoice>
            {
                new NewNoteChoice(),
                new OpenLoggedSshSessionChoice(_project),
                new ExitChoice(),
                new DeleteNotesAction()
            };

            var manageTargets = new ManageableCreatable(TargetManager.Instance).GetActions();
            var targets = TargetManager.Instance.Creatables.Select(t => new AutoAction(t, TargetManager.Instance));
            var a = targets.Concat(manageTargets);
            return c.Concat(a.Concat(NotesManager.Instance.GetNoteChoices()));
        }
    }
}