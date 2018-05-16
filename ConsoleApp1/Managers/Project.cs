using System;
using System.IO;

namespace ConsoleApp1.BuiltInActions
{
    internal class Project
    {
        private readonly ProjectPersistence _settings;

        public Project(ProjectPersistence settings)
        {
            _settings = settings;
        }

        public TargetManager TargetManager => _settings.TargetManager;
        public PortManager PortManager => _settings.PortManager;

        public string GetLogFileFullLocation()
        {
            var fileName = _settings.ProjectFolder + Path.DirectorySeparatorChar + GetLogFileName();
            _settings.NotesManager.NewLoggedNote(fileName, "SSH Session " + DateTime.Now.ToLocalTime());
            return fileName;
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

        public string GetLogFileFullLocation(UserAction userAction, string target)
        {
            var fileName = _settings.ProjectFolder + Path.DirectorySeparatorChar + GetLogFileName();
            _settings.NotesManager.NewLoggedNote(fileName, userAction.Name + " " + (target ?? "") + 
                DateTime.Now.ToLocalTime());
            return fileName;
        }
    }
}