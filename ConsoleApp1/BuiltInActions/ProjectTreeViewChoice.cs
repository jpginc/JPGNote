namespace ConsoleApp1.BuiltInActions
{
    internal class ProjectTreeViewChoice : SimpleTreeViewChoice
    {
        private readonly Project _project;

        public ProjectTreeViewChoice(Project project) : base("New Project")
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