using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class UserAction : ICreatable
    {
        [DataMember] [AutoSingleLineString] public string Name { get; set; } = "";

        [DataMember]
        [AutoSingleLineString]
        [Wizard]
        public string Command { get; set; } = "";

        [IgnoreDataMember] public string EditChoiceText => Name;
    }
}