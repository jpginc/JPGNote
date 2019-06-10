using System.Collections.Generic;

namespace ConsoleApp1.BuiltInActions
{
    internal class SetupCommandMenu : IActionProvider
    {
        private UserAction _userAction;
        private Project _project;
        private LoggedCommand _loggedCommand;

        public SetupCommandMenu(UserAction userAction, Project project)
        {
            _userAction = userAction;
            _project = project;
            _loggedCommand = new LoggedCommand(_userAction, _project);
        }

        public InputType InputType => InputType.Single;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            var retVal = new List<ITreeViewChoice>();
            if (_loggedCommand.NeedsTarget())
            {
                var selectTargetNotRun = new ChoiceToActionProvider(new SelectTargetMenu(_project, 
                    _loggedCommand), "Select TargetReference (not run)");
                var selectTargetAll = new ChoiceToActionProvider(new SelectTargetMenuAll(_project, 
                    _loggedCommand), "Select TargetReference (All)");
                retVal.Add(selectTargetNotRun);
                retVal.Add(selectTargetAll);
            }
            //todo check if userAction needs ports
            if (_loggedCommand.NeedsPort())
            {
                var selectPort = new ChoiceToActionProvider(new SelectPortMenu(_project, 
                    _loggedCommand), "Select Port");
                retVal.Add(selectPort);
            }
            //todo preview userAction
            //var previewChoice = new TOTO;
            //todo run userAction
            var runChoice = new ChoiceToCallback(() => _loggedCommand.Run(), "Run");
            retVal.Add(runChoice);
            return retVal;
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }
    }
}