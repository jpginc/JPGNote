using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class MachineManager
    {
        private static readonly string _sshLocation = "C:\\Program Files\\Git\\usr\\bin\\ssh.exe";

        [IgnoreDataMember] public static MachineManager Instance { get; set; } = new MachineManager();

        [DataMember] public List<SshAbleMachine> sshAbleMachines { get; set; } = new List<SshAbleMachine>();

        [IgnoreDataMember] private SshAbleMachine CurrentMachine => sshAbleMachines.First();

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
                var machine = new SshAbleMachine {Name = machineName.Result};
                sshAbleMachines.Add(machine);
                ProgramSettingsClass.Instance.Save();
                JpgActionManager.PushActionContext(new MachineMenu(machine));
            }
        }

        public string GetSshCommandLineString()
        {
            var str =
                $"\"{_sshLocation}\" \"-i\" \"{PutSshKeyInTempLocation()}\" " +
                $"\"{CurrentMachine.SshUserName}@{GetIpOrDomain()}\"";
            return str;
        }

        private string GetIpOrDomain()
        {
            //todo
            return CurrentMachine.IpOrDomainName;
        }

        private string PutSshKeyInTempLocation()
        {
            var tempLocation = Path.GetTempFileName();
            File.WriteAllText(tempLocation, CurrentMachine.SshKey);
            new Timer(DeleteTempFile, tempLocation, 1000, Timeout.Infinite);
            return tempLocation;
        }

        public void DeleteTempFile(object tempFile)
        {
            File.Delete((string)tempFile);
        }
    }
}