namespace ConsoleApp1.BuiltInActions
{
    internal class ProjectChoice : SimpleTreeViewChoice
    {
        private readonly ProgramProjectSetting _project;

        public ProjectChoice(ProgramProjectSetting project) : base(project.ProjectName)
        {
            _project = project;
            AcceptHandler = ProjectContext;
        }

        private void ProjectContext(UserActionResult obj)
        {
            var project = ProjectManager.Instance.LoadProject(_project.ProjectFolder, _project.ProjectName);
            JpgActionManager.PushActionContext(new ProjectAction(project));
        }
    }
}