using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Port : ICreatable, IComparable<Port>, IDoneable
    {
        [IgnoreDataMember] public string EditChoiceText => PortNumber;

        [DataMember, Wizard, AutoSingleLineString]
        public string PortNumber { get; set; } = "";
        [DataMember, AutoSingleLineString] public string Target { get; set; } = "";
        [IgnoreDataMember] public List<ICreatable> Notes
        {
            get
            {
                return NoteReferences.Select(r => ProgramSettingsClass.Instance.GetNote(r))
                    .Where(n => n != null)
                    .ToList();
            }
        }

        [DataMember] public List<string> NoteReferences { get; set; } = new List<string>();
        [DataMember] public List<string> CommandsRun { get; set; } = new List<string>();

        [DataMember] public ScanItemState ScanItemStatus { get; set; } = ScanItemState.NotSet;
        public int CompareTo(Port other)
        {
            return string.CompareOrdinal(PortNumber, other.PortNumber) 
                   + string.CompareOrdinal(Target, other.Target);
        }
    }

    internal interface IDoneable
    {
        ScanItemState ScanItemStatus { get; set; }
    }
}