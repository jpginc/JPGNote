using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class MachineManager
    {
        [IgnoreDataMember]
        public static MachineManager Instance { get; set; } = new MachineManager();

        [DataMember]
        public List<SshAbleMachine> sshAbleMachines { get; set; } = new List<SshAbleMachine>();

        public void ManageMachines()
        {
            JpgActionManager.PushActionContext(new MachineManagerMenu());
        }

        public IEnumerable<ITreeViewChoice> GetMachines()
        {
            return sshAbleMachines.Select(m => new MachineChoice(m));
        }

        public void CreateNewMachine(UserActionResult obj)
        {
            var machineName = GuiManager.Instance.GetNonEmptySingleLineInput("Set Machine Name");
            if (machineName.ResponseType == UserActionResult.ResultType.Accept)
            {
                var machine = new SshAbleMachine() {Name = machineName.Result};
                sshAbleMachines.Add(machine);
                ProgramSettingsClass.Instance.Save();
                JpgActionManager.PushActionContext(new MachineMenu(machine));
            }
        }
    }
}