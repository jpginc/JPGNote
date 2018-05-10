using System.Collections.Generic;

namespace ConsoleApp1.BuiltInActions
{
    internal class UserActionMenu : SimpleActionProvider
    {
        private UserAction userAction;

        public UserActionMenu(UserAction userAction)
        {
            this.userAction = userAction;
        }

        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            return AutoSetPropertyProvider.GetActions(userAction);
        }
    }
}