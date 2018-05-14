using System;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    public enum ScanItemState
    {
        NotSet,
        Done,
        ToDo,
        Vulnerable
    }
    [DataContract]
    internal class Port : ICreatable, IComparable<Port>, IDoneable
    {
        [IgnoreDataMember] public string EditChoiceText => PortNumber;
        public IManager Manager => PortManager.Instance;

        [DataMember, Wizard, AutoSingleLineString]
        public string PortNumber { get; set; } = "";
        [DataMember, AutoSingleLineString] public string Target { get; set; } = "";
        [DataMember, AutoSingleLineString] public string Notes { get; set; } = "";
        [DataMember] public ScanItemState ScanItemStatus { get; set; } = ScanItemState.NotSet;
        public int CompareTo(Port other)
        {
            var portsFirst = string.CompareOrdinal(PortNumber, other.PortNumber);
            return portsFirst != 0 ? portsFirst : string.CompareOrdinal(Target, other.Target);
        }
    }

    internal interface IDoneable
    {
        ScanItemState ScanItemStatus { get; set; }
    }
}