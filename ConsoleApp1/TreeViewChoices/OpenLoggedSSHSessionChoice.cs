namespace ConsoleApp1.BuiltInActions
{
    internal class OpenLoggedSshSessionChoice : SimpleTreeViewChoice
    {
        private readonly Project _p;

        public OpenLoggedSshSessionChoice(Project p) : base("Open Logged SSH Session")
        {
            _p = p;
            AcceptHandler = OpenSshSession;
        }

        private void OpenSshSession(UserActionResult obj)
        {
            CommandManager.Instance.OpenSshSession(_p);
        }
    }
}