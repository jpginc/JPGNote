namespace ConsoleApp1.BuiltInActions
{
    internal class MachineChoice : SimpleTreeViewChoice
    {
        private SshAbleMachine _machine;

        public MachineChoice(SshAbleMachine machine ) : base(machine.Name)
        {
            _machine = machine;
            AcceptHandler = SetMachineContext;
        }

        private void SetMachineContext(UserActionResult obj)
        {
            JpgActionManager.PushActionContext(new AutoMenu(_machine));
        }
    }
}