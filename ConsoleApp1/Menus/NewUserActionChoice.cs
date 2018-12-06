namespace ConsoleApp1.BuiltInActions
{
    internal class NewUserActionChoice : SimpleTreeViewChoice
    {
        public NewUserActionChoice() : base("Create New Action")
        {
            AcceptHandler = UserActionManager.Instance.CreateNewUserAction;
        }
    }

    internal class ExportUserActionChoice : SimpleTreeViewChoice
    {
        public ExportUserActionChoice() : base("Export actions")
        {
            AcceptHandler = UserActionManager.Instance.ExportActions;
        }
    }
    internal class ImportUserActionChoice : SimpleTreeViewChoice
    {
        public ImportUserActionChoice() : base("Import actions")
        {
            AcceptHandler = UserActionManager.Instance.ImportActions;
        }
    }

}