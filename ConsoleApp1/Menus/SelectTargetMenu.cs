using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class SelectTargetMenu : IActionProvider
    {
        private Project _project;
        private readonly LoggedCommand _loggedCommand;

        public SelectTargetMenu(Project project, LoggedCommand loggedCommand)
        {
            _project = project;
            _loggedCommand = loggedCommand;
        }

        public InputType InputType => InputType.Multi;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            var targets = _project.TargetManager.Creatables
                .Where(t => ! ((Target) t).CommandsRun.Contains(_loggedCommand.Name))
                .Select(t => new AutoAction(t, _project.TargetManager));
            //todo select all
            //todo regex/grep targets
            //todo use Port's target
            return targets;
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            if (res.Result == UserActionResult.ResultType.Accept)
            {
                _loggedCommand.SetTargets(res.UserChoices.Select(c => (Target) ((AutoAction) c).Creatable));
            }
            JpgActionManager.UnrollActionContext();
            return ActionProviderResult.ProcessingFinished;
        }
    }
}