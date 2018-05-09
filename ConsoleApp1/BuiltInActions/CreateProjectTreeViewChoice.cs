namespace ConsoleApp1.BuiltInActions
{
    internal class CreateProjectTreeViewChoice : SimpleTreeViewChoice
    {
        public CreateProjectTreeViewChoice() : base("New Project")
        {
            AcceptHandler = ProjectManager.Instance.NewProject;
        }
    }
}