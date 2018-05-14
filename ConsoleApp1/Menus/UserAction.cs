using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class UserAction : ICreatable
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

        [IgnoreDataMember] public string EditChoiceText => Name;
        public IManager Manager => MachineManager.Instance;
    }
}