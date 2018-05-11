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
            var commandString = CreateCommandString();
            CommandManager.Instance.RunCommand(commandString, _project);
            return true;
        }

        private string CreateCommandString()
        {
            var command = _userAction.Command;
            if (NeedsPort())
            {
                command = command.Replace(portMarker, _ports.First().PortNumber);
            }

            if (NeedsTarget())
            {
                command = command.Replace(targetMarker, _targets.First().IpOrDomain);
            }
            return command;
        }

        public bool NeedsTarget()
        {
            return _userAction.Command.Contains("{{TARGET}}");
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