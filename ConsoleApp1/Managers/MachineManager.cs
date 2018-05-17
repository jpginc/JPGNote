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
        [IgnoreDataMember] private static readonly string _sshLocation = "C:\\Program Files\\Git\\usr\\bin\\ssh.exe";
        [IgnoreDataMember] public string ManageText => "Manage Machines";
        [IgnoreDataMember] public string CreateChoiceText => "Create Machine";
        [IgnoreDataMember] public string DeleteChoiceText => "Delete Machines";
        [DataMember] public List<ICreatable> Creatables { get; set; } = new List<ICreatable>();
        [IgnoreDataMember] public static MachineManager Instance { get; set; } = new MachineManager();


        public void ManageMachines()
        {
            JpgActionManager.PushActionContext(new MachineManagerMenu());
        }

        public int MachineCount()
        {
            return Creatables.Count();
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
                var machine = new SshAbleMachine
                {
                    Name = machineName.Result,
                    RunningJobs = new List<JobDetails>()
                };
                Creatables.Add(machine);
                ProgramSettingsClass.Instance.Save();
                JpgActionManager.PushActionContext(new AutoMenu(machine, this));
            }
        }

        public string DontWorryAboutTooManySessions()
        {
            var str =
                $"\"{_sshLocation}\" " + GetSshStringFromMachine((SshAbleMachine) Creatables.First());
            return str;
        }

        public string GetSshCommandLineArgs(JobDetails toRun)
        {
            foreach (var machine in Creatables)
            {
                var m = (SshAbleMachine) machine;
                if (m.RunningJobs == null)
                {
                    m.RunningJobs = new List<JobDetails>();
                }
                if (m.RunningJobs.Count <= 15)
                {
                    m.RunningJobs.Add(toRun);
                    Console.WriteLine("running on " + m.Name);
                    return GetSshStringFromMachine(m);
                }
            }

            throw new Exception("machines isn't workgin");
        }

        private string GetSshStringFromMachine(SshAbleMachine m)
        {
            return $"\"-i\" \"{PutSshKeyInTempLocation(m)}\" " +
                   $"\"-o\" \"StrictHostKeyChecking=no\" " + 
                   $"\"{m.SshUserName}@{GetIpOrDomain(m)}\"";
        }

        private string GetIpOrDomain(SshAbleMachine m)
        {
            //todo
            return m.IpOrDomainName;
        }

        private string PutSshKeyInTempLocation(SshAbleMachine sshAbleMachine)
        {
            var tempLocation = Path.GetTempFileName();
            File.WriteAllText(tempLocation, sshAbleMachine.SshKey);
            new Timer(DeleteTempFile, tempLocation, 1000, Timeout.Infinite);
            return tempLocation;
        }

        public void DeleteTempFile(object tempFile)
        {
            File.Delete((string) tempFile);
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

        public void New(UserActionResult obj)
        {
            CreateNewMachine(obj);
        }

        public bool HasChildren()
        {
            return false;
        }

        public IEnumerable<ICreatable> GetChildren(ICreatable parent)
        {
            return Enumerable.Empty<ICreatable>();
        }

        public void JobDone(JobDetails job)
        {
            foreach (var machine in Creatables)
            {
                var m = (SshAbleMachine) machine;
                if (m.RunningJobs == null)
                {
                    m.RunningJobs = new List<JobDetails>();
                }
                if (m.RunningJobs.Contains(job))
                {
                    m.RunningJobs.Remove(job);
                    break;
                }
            }
        }
    }
}