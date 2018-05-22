namespace ConsoleApp1.BuiltInActions
{
    internal class NewMachineChoice : SimpleTreeViewChoice
    {
        public NewMachineChoice() : base("Setup New Machine")
        {
            AcceptHandler = MachineManager.Instance.CreateNewMachine;
        }
    }
}