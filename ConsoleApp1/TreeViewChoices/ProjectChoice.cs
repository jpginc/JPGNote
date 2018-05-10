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
            //this is where you load an already created 
            JpgActionManager.PushActionContext(new ProjectAction(_project));
        }
    }
}