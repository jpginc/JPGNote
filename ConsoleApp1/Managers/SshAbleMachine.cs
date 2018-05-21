using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{

    [DataContract]
    public class SshAbleMachine : ICreatable
    {
        [AutoSingleLineString, DataMember] public string MacAddress { get; set; } = "";

        [AutoMultiLineString, DataMember] public string SshKey { get; set; } = "";

        [AutoSingleLineString, DataMember] public string SshKeyPassphrase { get; set; } = "";

        [AutoSingleLineString, DataMember] public string IpOrDomainName { get; set; } = "";
        [AutoSingleLineString, DataMember] public string IsAvailable { get; set; } = "yes";

        [DataMember] public List<string> Tags { get; set; }

        [AutoSingleLineString, DataMember, Wizard] public string Name { get; set; } = "";
        [AutoSingleLineString, DataMember] public string SshUserName { get; set; } = "";
        [IgnoreDataMember] public string EditChoiceText => Name;
        [IgnoreDataMember] public List<JobDetails> RunningJobs { get; set; } = new List<JobDetails>();
    }
}