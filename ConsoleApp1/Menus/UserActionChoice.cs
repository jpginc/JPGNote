namespace ConsoleApp1.BuiltInActions
{
    internal class UserActionChoice : SimpleTreeViewChoice
    {
        private UserAction u;

        public UserActionChoice(UserAction u) : base(u.Name)
        {
            this.u = u;
            AcceptHandler = SetContext;
        }

        private void SetContext(UserActionResult obj)
        {
            JpgActionManager.PushActionContext(new AutoMenu(u, ProgramSettingsClass.Instance));
        }
    }
}