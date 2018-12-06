using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class UserActionManagerMenu : SimpleActionProvider
    {
        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            var userActions =  UserActionManager.Instance.GetUserActionChoices();
            var c = new List<ITreeViewChoice>
            {
                new NewUserActionChoice(),
                new ExportUserActionChoice(),
                new ImportUserActionChoice()
            };
            return userActions.Concat(c);
        }
    }
}