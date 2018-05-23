using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    public class UserAction : ICreatable
    {
        [DataMember]
        [AutoSingleLineString]
        [Wizard]
        public string Name { get; set; } = "";

        [DataMember]
        [AutoSingleLineString]
        [Wizard]
        public string Command { get; set; } = "";

        [DataMember] [AutoFilePicker] public string ParsingCodeLocation { get; set; } = "";
        [DataMember, AutoSingleLineString] public string IsInteractive { get; set; } = "no";
        [IgnoreDataMember] public string EditChoiceText => Name;
    }
}