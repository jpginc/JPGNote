using System.Collections.Generic;

namespace ConsoleApp1.BuiltInActions
{
    internal class MachineMenu : SimpleActionProvider
    {
        private readonly SshAbleMachine _machine;

        public MachineMenu(SshAbleMachine machine)
        {
            _machine = machine;
        }

        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            var c = new List<ITreeViewChoice>
            {
                new UpdateSingleLineStringAction(arg =>
                    {
                        _machine.Name = arg;
                        ProgramSettingsClass.Instance.Save();
                    }
                    , "Name", _machine.Name),
                new UpdateSingleLineStringAction(arg =>
                {
                    _machine.IpOrDomainName = arg;

                    ProgramSettingsClass.Instance.Save();
                }, "IpOrDomainName", _machine.IpOrDomainName),
                new UpdateSingleLineStringAction(arg =>
                {
                    _machine.MACAddress = arg;
                    ProgramSettingsClass.Instance.Save();
                }, "MACAddress", _machine.MACAddress),
                new UpdateSingleLineStringAction(arg =>
                {
                    _machine.SshKeyPassphrase = arg;
                    ProgramSettingsClass.Instance.Save();
                }, "SshKeyPassphrase", _machine.SshKeyPassphrase),
                new UpdateMultiLineStringAction(arg =>
                {
                    _machine.SshKey = arg;
                    ProgramSettingsClass.Instance.Save();
                }, "SshKey", _machine.SshKey)
            };

            return c;
        }
    }
}