using System;
using System.IO;

namespace ConsoleApp1.BuiltInActions
{
    internal class Project
    {
        public string Name { get; private set; }
        private string _folder;
        private string _password;
        private ProjectSettingsClass _settings;
        public string EditChoiceText => Name;
        public string CreateChoiceText => "Create New Project";
        public string DeleteChoiceText => "Delete Projects";

        public Project(string name, string folder, string password)
        {
            Initialise(name, folder, password);
        }

        private void Initialise(string name, string folder, string password)
        {
            Name = name;
            _folder = folder;
            _password = password;
            _settings = ProjectSettingsClass.Load(folder, password);
        }

        public string GetLogFileFullLocation()
        {
            var fileName = _folder + Path.DirectorySeparatorChar + GetLogFileName();
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
            throw new NotImplementedException();
        }

        public string GetLogFileFullLocation(UserAction userAction, string target)
        {
            var fileName = _folder + Path.DirectorySeparatorChar + GetLogFileName();
            _settings.NotesManager.NewLoggedNote(fileName, userAction.Name + " " + (target ?? ""));
            return fileName;
        }
    }
}