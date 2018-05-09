namespace ConsoleApp1.BuiltInActions
{
    internal class ProjectChoice : SimpleTreeViewChoice
    {
        private readonly Project _project;

        public ProjectChoice(Project project) : base("New Project")
        {
            _project = project;
            AcceptHandler = ProjectContext;
        }

        private void ProjectContext(UserActionResult obj)
        {
            JpgActionManager.PushActionContext(new ProjectAction(_project));
        }
    }
}