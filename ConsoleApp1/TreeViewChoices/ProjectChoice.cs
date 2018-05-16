namespace ConsoleApp1.BuiltInActions
{
    internal class ProjectChoice : SimpleTreeViewChoice
    {
        private readonly ProgramProjectSetting _project;
        private readonly ProjectManager _projectManager;

        public ProjectChoice(ProgramProjectSetting project, ProjectManager projectManager) 
            : base(project.ProjectName)
        {
            _project = project;
            _projectManager = projectManager;
            AcceptHandler = ProjectContext;
        }

        private void ProjectContext(UserActionResult obj)
        {
            var project = new Project(_projectManager.GetProjectPersistence(_project));
            JpgActionManager.PushActionContext(new ProjectAction(project));
        }
    }
}