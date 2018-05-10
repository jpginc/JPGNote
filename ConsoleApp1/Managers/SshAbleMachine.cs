using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    public class SshAbleMachine
    {
        [DataMember] public AutoSingleLineString MacAddress { get; set; } = new AutoSingleLineString("");

        [DataMember] public AutoMultiLineString SshKey { get; set; } = new AutoMultiLineString("");

        [DataMember] public AutoSingleLineString SshKeyPassphrase { get; set; } = new AutoSingleLineString("");

        [DataMember] public AutoSingleLineString IpOrDomainName { get; set; } = new AutoSingleLineString("");

        [DataMember] public List<string> Tags { get; set; }

        [DataMember] public AutoSingleLineString Name { get; set; } = new AutoSingleLineString("");
        [DataMember] public AutoSingleLineString SshUserName { get; set; } = new AutoSingleLineString("");
    }
}