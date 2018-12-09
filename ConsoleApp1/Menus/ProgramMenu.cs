using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class ProgramMenu : SimpleActionProvider
    {
        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            var projects = new ManageableCreatable(ProjectManager.Instance).GetActionsWithChildren();
            var userActions = new ManageableCreatable(UserActionManager.Instance).GetManageActions();
            var c = new List<ITreeViewChoice>
            {
                new ManageSlaveMachinesChoice(),
                new ExitChoice()
            };
            c.AddRange(new ManageableCreatable(OutputFilterManager.Instance).GetActions());
            return projects.Concat(c).Concat(userActions);
        }

    }
}