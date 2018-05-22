namespace ConsoleApp1.BuiltInActions
{
    internal class NewUserActionChoice : SimpleTreeViewChoice
    {
        public NewUserActionChoice() : base("Create New Action")
        {
            AcceptHandler = UserActionManager.Instance.CreateNewUserAction;
        }
    }
}