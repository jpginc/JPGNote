using System.Collections.Generic;

namespace ConsoleApp1.BuiltInActions
{
    internal class ProjectAction : IActionProvider
    {
        private Project _project;
        private readonly ProjectMainMenu _myMenu;

        public ProjectAction(Project project)
        {
            _project = project;
            _myMenu = new ProjectMainMenu();
        }

        public InputType InputType => _myMenu.InputType;
        public IEnumerable<ITreeViewChoice> GetActions() => _myMenu.GetActions();

        public ActionProviderResult HandleUserAction(UserActionResult res) => _myMenu.HandleUserAction(res);
    }
}