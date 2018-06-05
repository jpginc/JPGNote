using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    public class UserAction : BaseCreatable
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
        [IgnoreDataMember] public override string EditChoiceText => Name;
        [IgnoreDataMember] public override string ThisSummary => $"Name: {Name}\nCommand: {Command}\n";
        [IgnoreDataMember] public override List<string> ChildrenReferences => new List<string>();
    }
}