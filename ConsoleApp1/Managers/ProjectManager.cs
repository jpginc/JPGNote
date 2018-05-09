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
        [DataMember] private List<Project> Projects { get; } = new List<Project>();
        private ProjectManager()
        {
        }

        public static ProjectManager Instance { get; } = new ProjectManager();

        public IEnumerable<ITreeViewChoice> GetProjects()
        {
            return Projects.Select(p => new ProjectChoice(p));
        }

        public void NewProject(UserActionResult obj)
        {
            Console.WriteLine(GuiManager.Instance.GetFolder("Select Project Folder").Result);
        }
    }
}