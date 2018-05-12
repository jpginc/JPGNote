namespace ConsoleApp1.BuiltInActions
{
    internal class ManageUserActionsChoice : SimpleTreeViewChoice
    {
        public ManageUserActionsChoice() : base("Manage User Defined Actions")
        {
            AcceptHandler = ManageUserActions;
        }

        private void ManageUserActions(UserActionResult obj)
        {
            UserActionManager.Instance.ManageUserActions();
        }
    }
}