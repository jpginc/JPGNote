using System;
using System.IO;

namespace ConsoleApp1.BuiltInActions
{
    public class Project
    {
        private readonly ProjectPersistence _settings;

        public Project(ProjectPersistence settings)
        {
            _settings = settings;
            _settings.Project = this;
        }

        public TargetManager TargetManager => _settings.TargetManager;
        public NotesManager NotesManager => _settings.NotesManager;
        public PortManager PortManager => _settings.PortManager;
        public CommandQueue CommandQueue => _settings.CommandQueue;
        public TagManager TagManager => _settings.TagManager;

        public LoggedNote GetLogFileFullLocation()
        {
            var fileName = _settings.ProjectFolder + Path.DirectorySeparatorChar + GetLogFileName();
            var note = _settings.NotesManager.NewLoggedNote(fileName, "SSH Session " + DateTime.Now.ToLocalTime());
            return note;
        }

        private string GetLogFileName()
        {
            return DateTime.Now.ToFileTimeUtc().ToString();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
                _settings.Save();
        }

        public LoggedNote GetLogFileFullLocation(UserAction userAction, Target target)
        {
            var fileName = _settings.ProjectFolder + Path.DirectorySeparatorChar + GetLogFileName();
            var note = _settings.NotesManager.NewLoggedNote(fileName, userAction.Name + " " + (target?.IpOrDomain ?? "") + 
                DateTime.Now.ToLocalTime());
            return note;
        }

        public void CommandDone(JobDetails job)
        {
            CommandQueue.Remove(job);
        }

        public void CommandQueued(JobDetails jobDetails)
        {
            CommandQueue.Add(jobDetails);
        }
    }
}