using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{

    [DataContract]
    public class SshAbleMachine : BaseCreatable
    {
        [AutoSingleLineString, DataMember] public string MacAddress { get; set; } = "";

        [AutoMultiLineString, DataMember] public string SshKey { get; set; } = "";

        [AutoSingleLineString, DataMember] public string SshKeyPassphrase { get; set; } = "";

        [AutoSingleLineString, DataMember] public string IpOrDomainName { get; set; } = "";
        [IgnoreDataMember] public string SessionIpOrDomainName { get; set; } = "";
        [AutoSingleLineString, DataMember] public string IsAvailable { get; set; } = "yes";

        [DataMember] public List<string> Tags { get; set; }

        [AutoSingleLineString, DataMember] public string Name { get; set; } = "";
        [AutoSingleLineString, DataMember] public string SshUserName { get; set; } = "";
        [IgnoreDataMember] public override string EditChoiceText => Name;
        [IgnoreDataMember] public override List<string> ChildrenReferences => new List<string>();

        [IgnoreDataMember]
        public override string ThisSummary => $"Name: {Name}\nSSH Username: {SshUserName}\nIP or domain: {IpOrDomainName}";

        [IgnoreDataMember] public List<JobDetails> RunningJobs { get; set; } = new List<JobDetails>();
        public IManager Manager => MachineManager.Instance;

        [IgnoreDataMember] public bool HasMac => MacAddress != null && ! MacAddress.Trim().Equals("");
        [IgnoreDataMember] public bool HasIpOrDomainName => IpOrDomainName != null && ! IpOrDomainName.Trim().Equals("");
        [IgnoreDataMember] public bool HasSessionIpOrDomainName => SessionIpOrDomainName != null && ! SessionIpOrDomainName.Trim().Equals("");
    }
}