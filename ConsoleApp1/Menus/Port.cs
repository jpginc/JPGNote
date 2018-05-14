using System;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class Port : ICreatable, IComparable<Port>
    {
        [IgnoreDataMember] public string EditChoiceText => PortNumber;
        public IManager Manager => PortManager.Instance;

        [DataMember, Wizard, AutoSingleLineString]
        public string PortNumber { get; set; } = "";
        [DataMember, AutoSingleLineString] public string Target { get; set; } = "";
        [DataMember, AutoSingleLineString] public string Notes { get; set; } = "";
        public int CompareTo(Port other)
        {
            var portsFirst = string.CompareOrdinal(PortNumber, other.PortNumber);
            return portsFirst != 0 ? portsFirst : string.CompareOrdinal(Target, other.Target);
        }
    }
}