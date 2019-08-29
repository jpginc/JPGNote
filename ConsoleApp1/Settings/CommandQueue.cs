using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ConsoleApp1.BuiltInActions;

namespace ConsoleApp1
{
    [DataContract]
    public class CommandQueue
    {
        /*
        [DataMember] public List<SerialisableJob> JobDetails = new List<SerialisableJob>();
        [IgnoreDataMember] private List<SerialisableJob> _seperateFromNewQueue;

        [IgnoreDataMember] private bool _haveAlreadyRevived = false;

        public void Add(JobDetails job)
        {
            if (!_haveAlreadyRevived)
            {
                _seperateFromNewQueue = JobDetails.Select(a => a).ToList();
            }
            JobDetails.Add(new SerialisableJob(job));
            job.Project.Save();
        }

        public void Remove(JobDetails job)
        {
            JobDetails.Remove(new SerialisableJob(job));
            job.Project.Save();
        }

        public void Revive(Project project)
        {
            if (_haveAlreadyRevived)
            {
                return;
            }
            _haveAlreadyRevived = true;
            if (JobDetails == null)
            {
                JobDetails = new List<SerialisableJob>();
            }

            var enumerated = _seperateFromNewQueue?.ToArray() ?? JobDetails.ToArray();
            foreach (var j in enumerated)
            {
                var userAction = UserActionManager.Instance.GetAction(j.UserActionName);
                var port = project.PortManager.GetPort(j.PortNumber);
                var target = project.TargetManager.GetTarget(j.TargetName);
                if (userAction == null)
                {
                    Console.WriteLine("Job didn't have a valid user action");
                    continue;
                }

                CommandManager.Instance.ReviveCommand(new JobDetails(j.CommandString, target, 
                    port, userAction, project));
            }
        }

        public void ClearQueue(Project project)
        {
            JobDetails.Clear();
            project.Save();
        }
    }

    [DataContract]
    public class SerialisableJob : IEquatable<SerialisableJob>
    {
        [DataMember] public  string UserActionName { get; set; }
        [DataMember] public string TargetName { get; set; }
        [DataMember] public string PortNumber { get; set; }
        [DataMember] public string CommandString { get; set; }

        public SerialisableJob(JobDetails job)
        {
            UserActionName = job.UserAction.Name;
            TargetName = job.Target?.IpOrDomain;
            PortNumber = job.Port?.PortNumber;
            CommandString = job.CommandString;
        }

        public bool Equals(SerialisableJob other)
        {
            return Equals(UserActionName, other.UserActionName)
                   && Equals(TargetName, other.TargetName)
                   && Equals(PortNumber, other.PortNumber)
                   && Equals(CommandString, other.CommandString);
        }
 */
    }
}