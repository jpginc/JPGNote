using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GLib;
using Gtk;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class ProjectManager
    {
        //a list of project names and their folders
        [DataMember] private List<ProgramProjectSetting> ProjectSettings { get; set; } = new List<ProgramProjectSetting>();
        //a list of actual projects, they are serialised into their own project folder
        [IgnoreDataMember] private List<Project> Projects { get; } = new List<Project>();
        [IgnoreDataMember] public static ProjectManager Instance { get; set; }
        public ProjectManager()
        {
            Instance = this;
        }


        public IEnumerable<ITreeViewChoice> GetProjects()
        {
            return ProjectSettings.Select(p => new ProjectChoice(p));
        }

        public void NewProject(UserActionResult obj)
        {
            var name = GuiManager.Instance.GetNonEmptySingleLineInput("Set ProgramProjectSetting Name");
            if (name.ResponseType != UserActionResult.ResultType.Accept)
            {
                return;
            }
            var folder = GuiManager.Instance.GetFolder("Select ProgramProjectSetting Folder");
            if (folder.ResponseType != UserActionResult.ResultType.Accept)
            {
                return;
            }

            ProjectSettings.Add(new ProgramProjectSetting(name.Result, folder.Result));
            var project = new Project(name.Result, folder.Result, ProgramSettingsClass.Instance.Password, true);
            Projects.Add(project);
            ProgramSettingsClass.Instance.Save();
        }
    }
}