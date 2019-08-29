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

        public string UniqueId => _settings.UniqueId ?? "";

        public TargetManager TargetManager => _settings.TargetManager;
        public NotesManager NotesManager => _settings.NotesManager;
        public PortManager PortManager => _settings.PortManager;
        public CommandQueue CommandQueue => _settings.CommandQueue;
        public TagManager TagManager => _settings.TagManager;

        public LoggedNote GetLogFileFullLocation(string userActionName = "SSH Session")
        {
            var fileName = _settings.ProjectFolder + Path.DirectorySeparatorChar + GetLogFileName();
            var note = _settings.NotesManager.NewLoggedNote(fileName, userActionName + DateTime.Now.ToLocalTime());
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

        public string GetLogFileFullLocation(UserAction userAction, Target target)
        {
            return _settings.ProjectFolder + Path.DirectorySeparatorChar + GetLogFileName();
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