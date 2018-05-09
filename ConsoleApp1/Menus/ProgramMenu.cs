using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class ProgramMenu : SimpleActionProvider
    {
        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            var projects = ProjectManager.Instance.GetProjects();
            var c = new List<ITreeViewChoice>
            {
                new NewProjectChoice(),
                new ExitChoice(),
            };
            return projects.Concat(c);
        }

    }
}