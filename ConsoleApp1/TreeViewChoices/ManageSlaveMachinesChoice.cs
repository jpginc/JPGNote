namespace ConsoleApp1.BuiltInActions
{
    internal class ManageSlaveMachinesChoice : SimpleTreeViewChoice
    {
        public ManageSlaveMachinesChoice() : base("Manage SSHable Machines")
        {
            AcceptHandler = ManageSlaveMachines;
        }

        private void ManageSlaveMachines(UserActionResult obj)
        {
            UserNotifier.Error("not implemented yet!");
        }
    }
}