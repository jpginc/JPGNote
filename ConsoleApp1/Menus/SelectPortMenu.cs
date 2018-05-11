using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class SelectPortMenu : IActionProvider
    {
        private Project _project;
        private LoggedCommand _loggedCommand;

        public SelectPortMenu(Project project, LoggedCommand loggedCommand)
        {
            _project = project;
            _loggedCommand = loggedCommand;
        }

        public InputType InputType => InputType.Single;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            var ports = TargetManager.Instance.GetActions();
            //todo select all
            //todo regex/grep targets
            //todo use Port's target
            return ports;
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            if (res.Result == UserActionResult.ResultType.Accept)
            {
                _loggedCommand.SetPorts(res.UserChoices.Select(c => (Port)((AutoAction)c).Creatable));
            }
            JpgActionManager.UnrollActionContext();
            return ActionProviderResult.ProcessingFinished;
        }
    }
}