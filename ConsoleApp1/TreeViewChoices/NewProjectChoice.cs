namespace ConsoleApp1.BuiltInActions
{
    internal class NewProjectChoice : SimpleTreeViewChoice
    {
        public NewProjectChoice() : base("New Project")
        {
            AcceptHandler = ProjectManager.Instance.NewProject;
        }
    }
}