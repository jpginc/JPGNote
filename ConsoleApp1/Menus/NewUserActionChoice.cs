namespace ConsoleApp1.BuiltInActions
{
    internal class NewUserActionChoice : SimpleTreeViewChoice
    {
        public NewUserActionChoice() : base("New Action")
        {
            AcceptHandler = obj =>
            {
                var userAction = new UserAction();
                if (CreatableWizard.GetRequiredFields(userAction))
                {
                    UserActionManager.Instance.Creatables.Add(userAction);
                    UserActionManager.Instance.Save();
                    JpgActionManager.PushActionContext(new AutoMenu(userAction, UserActionManager.Instance));
                }
            };
        }
    }
}