using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class UserActionManager
    {
        [IgnoreDataMember]
        public static UserActionManager Instance { get; set; } = new UserActionManager();
        [DataMember] public List<UserAction> UserActions { get; set; } = new List<UserAction>();

        public void ManageUserActions()
        {
            JpgActionManager.PushActionContext(new UserActionManagerMenu());
        }

        public IEnumerable<ITreeViewChoice> GetUserActionChoices()
        {
            return UserActions.Select(u=> new UserActionChoice(u));
        }

        public void CreateNewUserAction(UserActionResult obj)
        {
            var actionName = GuiManager.Instance.GetNonEmptySingleLineInput("Set Action Name");
            if (actionName.ResponseType == UserActionResult.ResultType.Accept)
            {
                var userAction = new UserAction() {Name = actionName.Result};
                UserActions.Add(userAction);
                ProgramSettingsClass.Instance.Save();
                JpgActionManager.PushActionContext(new AutoMenu(userAction, ProgramSettingsClass.Instance));
            }
        }
    }
}