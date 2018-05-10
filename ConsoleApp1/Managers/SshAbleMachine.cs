using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    public class SshAbleMachine
    {
        [DataMember]
        public string MACAddress { get; set; }
        [DataMember]
        public string SshKey { get; set; }
        [DataMember]
        public string SshKeyPassphrase { get; set; }
        [DataMember]
        public string IpOrDomainName { get; set; }
        [DataMember]
        public List<string> Tags { get; set; }

        [DataMember] public string Name { get; set; }
    }
}