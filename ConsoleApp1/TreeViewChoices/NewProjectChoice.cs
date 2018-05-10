namespace ConsoleApp1.BuiltInActions
{
    internal class NewProjectChoice : SimpleTreeViewChoice
    {
        public NewProjectChoice() : base("New ProgramProjectSetting")
        {
            AcceptHandler = ProjectManager.Instance.NewProject;
        }
    }
}