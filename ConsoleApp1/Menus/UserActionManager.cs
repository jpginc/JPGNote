using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    [KnownType(typeof(UserAction))]
    internal class UserActionManager : IManager
    {
        [IgnoreDataMember]
        public static UserActionManager Instance { get; set; } = new UserActionManager();
        [DataMember] public List<ICreatable> Creatables { get; set; } = new List<ICreatable>();

        public void ManageUserActions()
        {
            JpgActionManager.PushActionContext(new UserActionManagerMenu());
        }

        public IEnumerable<ITreeViewChoice> GetUserActionChoices()
        {
            return Creatables.Select(u=> new AutoAction(u,this));
        }

        public void CreateNewUserAction(UserActionResult obj)
        {
            //todo wizard
            var actionName = GuiManager.Instance.GetNonEmptySingleLineInput("Set Action Name");
            if (actionName.ResponseType == UserActionResult.ResultType.Accept)
            {
                var userAction = new UserAction() {Name = actionName.Result};
                Creatables.Add(userAction);
                ProgramSettingsClass.Instance.Save();
                JpgActionManager.PushActionContext(new AutoMenu(userAction, this));
            }
        }

        [IgnoreDataMember] public string ManageText => "Manage Actions";
        [IgnoreDataMember] public string CreateChoiceText => "Create New Action";
        [IgnoreDataMember]
        public string DeleteChoiceText => "Delete Actions";
        public void Save()
        {
            ProgramSettingsClass.Instance.Save();
        }

        public void Delete(ICreatable creatable)
        {
            Creatables.Remove(creatable);
            Save();
        }

        public void New(UserActionResult obj) => CreateNewUserAction(obj);
    }
}