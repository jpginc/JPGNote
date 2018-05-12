using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class SelectCommandToRunMenu : IActionProvider
    {
        private readonly Project _project;

        public SelectCommandToRunMenu(Project project)
        {
            _project = project;
        }

        public InputType InputType => InputType.Single;

        public IEnumerable<ITreeViewChoice> GetActions()
        {
            var userActions =  UserActionManager.Instance.GetUserActionChoices();
            return userActions;
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            if (res.Result == UserActionResult.ResultType.Accept && res.UserChoices.Count() != 0)
            {
                var command = (UserAction) ((AutoAction) res.UserChoices.First()).Creatable;
                JpgActionManager.PushActionContext(new SetupCommandMenu(command, _project));
            }
            else
            {
                JpgActionManager.UnrollActionContext();
            }
            return ActionProviderResult.ProcessingFinished;
        }
    }
}