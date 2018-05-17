using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class SelectCommandToRunMenu : IActionProvider
    {
        private readonly Project _project;
        private IEnumerable<Port> _prepopulatedPort;

        public SelectCommandToRunMenu(Project project)
        {
            _project = project;
        }

        public InputType InputType => InputType.Single;

        public IEnumerable<ITreeViewChoice> GetActions()
        {
            if (_prepopulatedPort == null)
            {
                var userActions = UserActionManager.Instance.GetUserActionChoices();
                return userActions;
            }

            return UserActionManager.Instance.GetPortCommands();
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            if (res.Result == UserActionResult.ResultType.Accept && res.UserChoices.Count() != 0)
            {
                var command = (UserAction) ((AutoAction) res.UserChoices.First()).Creatable;
                if (_prepopulatedPort == null)
                {
                    var menu = new SetupCommandMenu(command, _project);
                    JpgActionManager.PushActionContext(menu);
                }
                else
                {
                    var cmd = new LoggedCommand(command, _project);
                    cmd.SetPorts(_prepopulatedPort);
                    cmd.Run();
                }
            }
            else
            {
                JpgActionManager.UnrollActionContext();
            }
            return ActionProviderResult.ProcessingFinished;
        }

        public void PrepopulatePorts(IEnumerable<Port> ports)
        {
            _prepopulatedPort = ports;
        }
    }
}