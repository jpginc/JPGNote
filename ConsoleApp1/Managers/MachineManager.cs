using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    [KnownType(typeof(SshAbleMachine))]
    internal class MachineManager : IManager
    {
        [IgnoreDataMember]
        private static readonly string _sshLocation = "C:\\Program Files\\Git\\usr\\bin\\ssh.exe";
        [IgnoreDataMember] public string ManageText => "Manage Machines";
        [IgnoreDataMember] public string CreateChoiceText => "Create Machine";
        [IgnoreDataMember] public string DeleteChoiceText => "Delete Machines";
        [DataMember] public List<ICreatable> Creatables { get; set; } = new List<ICreatable>();
        [IgnoreDataMember] public static MachineManager Instance { get; set; } = new MachineManager();


        [IgnoreDataMember] private SshAbleMachine CurrentMachine => (SshAbleMachine) Creatables.First();

        public void ManageMachines()
        {
            JpgActionManager.PushActionContext(new MachineManagerMenu());
        }

        public IEnumerable<ITreeViewChoice> GetMachineChoices()
        {
            return Creatables.Select(m => new AutoAction(m, this));
        }

        public void CreateNewMachine(UserActionResult obj)
        {
            var machineName = GuiManager.Instance.GetNonEmptySingleLineInput("Set Machine Name");
            if (machineName.ResponseType == UserActionResult.ResultType.Accept)
            {
                SshAbleMachine machine = new SshAbleMachine {Name = machineName.Result};
                Creatables.Add(machine);
                ProgramSettingsClass.Instance.Save();
                JpgActionManager.PushActionContext(new AutoMenu(machine, this));
            }
        }

        public string GetSshCommandLineString()
        {
            var str =
                $"\"{_sshLocation}\" " + GetSshCommandLineArgs();
            return str;
        }

        public string GetSshCommandLineArgs()
        {
            var str = $"\"-i\" \"{PutSshKeyInTempLocation()}\" " +
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

        public void Save()
        {
            ProgramSettingsClass.Instance.Save();
        }

        public void Delete(ICreatable creatable)
        {
            Creatables.Remove(creatable);
            Save();
        }

        public void New(UserActionResult obj) => CreateNewMachine(obj);
    }
}