using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ConsoleApp1.BuiltInActions;

namespace ConsoleApp1
{
    /*
    public interface INewAction
    {
        string ListViewDisplayText {get;}
        void Callback(MainGui gui);
    }
    public class NewProjectAction : INewAction
    {
        public string ListViewDisplayText => "New Project";

        public void Callback(MainGui gui) => CreateNewProject(gui);

        public void CreateNewProject(MainGui gui)
        {
            var result = gui.GetNonEmptySingleLineInput("Enter name for project");
            if(result.ResponseType == UserActionResult.ResultType.Accept) {
                var newProject = new Project(result.Result);
                gui.Notify("Project Created");
            }
        }
    }

    public class Project : NewNote
    {
        private const string PROJECT_TYPE_NAME = "Project";
        public Project(string name) {
            ListViewDisplayText = name;
            TypeName = PROJECT_TYPE_NAME;
        }
    }
 */
}
 