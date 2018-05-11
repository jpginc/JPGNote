using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class Port : ICreatable
    {
        [IgnoreDataMember] public string EditChoiceText => PortNumber;
        [DataMember, Wizard, AutoSingleLineString] public string PortNumber { get; set; }
        [DataMember, AutoSingleLineString] public string Target { get; set; }
    }
}