using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }

        public void CreateNewUserAction(UserActionResult obj)
        {
            var actionName = GuiManager.Instance.GetNonEmptySingleLineInput("Set Action Name");
            if (actionName.ResponseType == UserActionResult.ResultType.Accept)
            {
                var userAction = new UserAction();
                UserActions.Add(userAction);
                ProgramSettingsClass.Instance.Save();
                JpgActionManager.PushActionContext(new AutoMenu(userAction));
            }
        }
    }
}