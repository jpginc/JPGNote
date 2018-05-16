using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class ProjectManager : Manager, IManagerAndActionProvider
    {
        [IgnoreDataMember] private readonly List<ProjectPersistence> _loadedProjects = new List<ProjectPersistence>();

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
                if (CreateProject(proj))
                {
                    Creatables.Add(proj);
                    Save();
                }
                UserNotifier.Notify("Project Created");
            }
        }

        private bool CreateProject(ProgramProjectSetting proj)
        {
            var folder = ProgramSettingsClass.FolderName;
            var password = ProgramSettingsClass.Instance.Password;
            var projectData = new ProjectPersistence();
            if (projectData.Create(folder, proj.ProjectName, password))
            {
                _loadedProjects.Add(projectData);
                return true;
            }
            return false;
        }

        public ProjectPersistence GetProjectPersistence(ProgramProjectSetting proj)
        {
            ProjectPersistence projectData = GetLoadedProject(proj);
            if (projectData == null)
            {
                var folder = ProgramSettingsClass.FolderName;
                var password = ProgramSettingsClass.Instance.Password;
                projectData = new ProjectPersistence();
                if (projectData.Load(folder, proj.ProjectName, password))
                {
                    _loadedProjects.Add(projectData);
                }                
            }

            return projectData;
        }

        private ProjectPersistence GetLoadedProject(ProgramProjectSetting projectSettings)
        {
            return _loadedProjects.FirstOrDefault(p => p.ProjectName.Equals(projectSettings.ProjectName));
        }

        public InputType InputType => InputType.Single;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return Creatables.Select(c =>new ProjectChoice((ProgramProjectSetting)c, this));
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }

        public override void New(UserActionResult obj) => NewProject(obj);
    }
}