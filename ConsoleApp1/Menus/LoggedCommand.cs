using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class LoggedCommand
    {
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
            if (NeedsPort())
            {
                foreach (var port in _ports)
                {
                    var commandString = CreateCommandString(port.Target, port.PortNumber);
                    CommandManager.Instance.RunCommand(commandString, _project, _userAction, port.Target, port);
                }
            } else if (NeedsTarget())
            {
                foreach (var target in _targets)
                {
                    var commandString = CreateCommandString(target.IpOrDomain, "");
                    CommandManager.Instance.RunCommand(commandString, _project, _userAction, target.IpOrDomain, null);
                }
            }
            else
            {
                var commandString = CreateCommandString("", "");
                CommandManager.Instance.RunCommand(commandString, _project, _userAction, "", null);
            }

            JpgActionManager.UnrollActionContext();
            return true;
        }

        private string CreateCommandString(string target, string port)
        {
            var command = _userAction.Command;
            command = command.Replace(portMarker, port);
            command = command.Replace(targetMarker, target);
            return command;
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