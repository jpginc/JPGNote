using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class LoggedCommand
    {
        private const string userInputMarker = "{{USERINPUT}}";
        private readonly UserAction _userAction;
        private Project _project;
        private IEnumerable<Target> _targets;
        private IEnumerable<Port> _ports;
        private string portMarker = "{{PORT}}";
        private string targetMarker = "{{TARGET}}";

        public LoggedCommand(UserAction userAction, Project project)
        {
            _userAction = userAction;
            _project = project;
        }

        public bool Run()
        {
            string commandString = ReplaceUserInput(_userAction.Command);
            if (NeedsPort())
            {
                foreach (var port in _ports)
                {
                    commandString = CreateCommandString(commandString, port.Target, port.PortNumber);
                    CommandManager.Instance.RunCommand(commandString, _project, _userAction, port.Target, port);
                }
            } else if (NeedsTarget())
            {
                foreach (var target in _targets)
                {
                    commandString = CreateCommandString(commandString, target.IpOrDomain, "");
                    CommandManager.Instance.RunCommand(commandString, _project, _userAction, target.IpOrDomain, null);
                }
            }
            else
            {
                commandString = CreateCommandString(commandString, "", "");
                CommandManager.Instance.RunCommand(commandString, _project, _userAction, "", null);
            }

            JpgActionManager.UnrollActionContext();
            return true;
        }

        private string CreateCommandString(string command, string target, string port)
        {
            command = command.Replace(portMarker, port);
            command = command.Replace(targetMarker, target);
            return command;
        }

        private string ReplaceUserInput(string command)
        {
            if (command.Contains(userInputMarker))
            {
                var input = GuiManager.Instance.GetNonEmptySingleLineInput("Provide input for " + command);
                if (input.ResponseType == UserActionResult.ResultType.Accept)
                {
                    return command.Replace(userInputMarker, input.Result);
                }
            }
            return null;
        }

        public bool NeedsTarget()
        {
            return _userAction.Command.Contains("{{TARGET}}") && ! NeedsPort();
        }

        public bool NeedsPort()
        {
            return _userAction.Command.Contains("{{PORT}}");
        }

        public void SetTargets(IEnumerable<Target> targets)
        {
            _targets = targets;
        }

        public void SetPorts(IEnumerable<Port> ports)
        {
            _ports = ports;
        }
    }
}