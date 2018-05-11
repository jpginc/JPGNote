using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class ProjectManager : Manager, IManagerAndActionProvider
    {
        //a list of project names and their folders
        [IgnoreDataMember] public override string ManageText => "Manage Projects";
        [IgnoreDataMember] public override string CreateChoiceText => "New Project";
        [IgnoreDataMember] public override string DeleteChoiceText => "Delete Projects";
        [IgnoreDataMember] public static ProjectManager Instance { get; set; }
        public void NewProject(UserActionResult obj)
        {
            var proj = new ProgramProjectSetting();
            if (CreatableWizard.GetRequiredFields(proj))
            {
                Creatables.Add(proj);
                Save();
                LoadProject(proj);
                UserNotifier.Notify("Project Created");
            }
        }

        private void LoadProject(ProgramProjectSetting projectSettings)
        {
            LoadProject(projectSettings.ProjectFolder, projectSettings.ProjectName);
        }

        public Project LoadProject(string folder, string name)
        {
            return new Project(name, folder, ProgramSettingsClass.Instance.Password);
        }

        public InputType InputType => InputType.Single;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return Creatables.Select(c =>new ProjectChoice((ProgramProjectSetting)c));
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }

        public override void New(UserActionResult obj) => NewProject(obj);
    }
}