﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    public class PortManager : Manager, IManagerAndActionProvider 
    {
        private Project _project;
        [IgnoreDataMember] public override string ManageText => "Manage Ports";
        [IgnoreDataMember] public override string CreateChoiceText => "New Port";
        [IgnoreDataMember] public override string DeleteChoiceText => "Delete Ports";
        public override void New(UserActionResult obj)
        {
            var port = new Port();
            if (CreatableWizard.GetRequiredFields(port))
            {
                Creatables.Add(port);
                Save();
            }
        }

        public InputType InputType => InputType.Multi;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return Creatables.Where(c => ((IDoneable) c).ScanItemStatus != ScanItemState.Done)
                .Select(c => new AutoAction(c, this));
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            if (res.Result == UserActionResult.ResultType.Accept
                && res.UserChoices.Count() > 1)
            {
                var x = new SelectCommandToRunMenu(Settings.Project);
                x.PrepopulatePorts(res.UserChoices.Select(c => ((AutoAction)c).Creatable as Port));
                JpgActionManager.PushActionContext(x);
                return ActionProviderResult.ProcessingFinished;
            }
            return ActionProviderResult.PassToTreeViewChoices;
        }

        public void AddPremade(Port port)
        {
            ICreatable existing = GetExistingOrNull(port);
            if (existing == null)
            {
                Creatables.Add(port);
            }
            else
            {
                ((Port)existing).Notes.AddRange(port.Notes);
            }
        }

        private Port GetExistingOrNull(Port port)
        {
            return Creatables.FirstOrDefault(p =>
            {
                var c = (Port)p;
                return c.TargetReference.Equals(port.TargetReference) && c.PortNumber.Equals(port.PortNumber);
            }) as Port;
        }

        public IEnumerable<ICreatable> GetChildren(Target target)
        {
            return Creatables.Where(c => ((Port) c).Target.Equals(target.IpOrDomain));
        }

        public Port GetPort(string portNumber)
        {
            return (Port) Creatables.FirstOrDefault(c => ((Port) c).PortNumber.Equals(portNumber));
        }

        public Port GetOrCreatePort(string portNumber, Target target)
        {
            var port = new Port()
            {
                PortNumber = portNumber,
                TargetReference = target.UniqueId
            };
            var existing = GetExistingOrNull(port);
            if (existing != null)
            {
                return existing;
            }

            Creatables.Add(port);
            target.ChildrenReferences.Add(port.UniqueId);
            return port;
        }

        public Port GetPortById(string uid)
        {
            return Creatables.FirstOrDefault(c => ((Port) c).UniqueId.Equals(uid)) as Port;
        }
    }
}