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
        [IgnoreDataMember] public string EditChoiceText => Target.Equals("") ? PortNumber : $"{PortNumber} ({Target})";

        [DataMember, Wizard, AutoSingleLineString]
        public string PortNumber { get; set; } = "";
        [DataMember, AutoSingleLineString] public string TargetReference { get; set; } = "";

        [IgnoreDataMember] public string Target => ProgramSettingsClass.Instance.GetTarget(TargetReference).IpOrDomain;
        [DataMember] public string UniqueId { get; set; } = Guid.NewGuid().ToString("N");

        [IgnoreDataMember]
        public string TagsString
        {
            get
            {
                var tags = string.Join(", ", Tags.Select(t => t.TagName));
                return tags.Equals("") ? "": $": {tags}";
            }
        }

        [IgnoreDataMember]
        public IEnumerable<Tag> Tags => ChildrenReferences
            .Select(r => ProgramSettingsClass.Instance.GetTag(r))
            .Where(t => t != null);

        [IgnoreDataMember] public List<Note> Notes
        {
            get
            {
                return ChildrenReferences.Select(r => ProgramSettingsClass.Instance.GetNote(r))
                    .Where(n => n != null)
                    .ToList();
            }
        }

        [DataMember] public List<string> ChildrenReferences { get; set; } = new List<string>();
        [IgnoreDataMember] public string ThisSummary => PortNumber;

        public string FullSummary =>
            $"Port number: {PortNumber}\nTarget: {Target}\nTags: {TagsString}{NotesSummaryOrEmpty()}";

        public string SummaryForParent => NotesSummaryOrEmpty();

        [DataMember] public List<string> CommandsRun { get; set; } = new List<string>();

        [DataMember] public ScanItemState ScanItemStatus { get; set; } = ScanItemState.NotSet;

        private string NotesSummaryOrEmpty()
        {
            var notes = Notes;
            return notes.Count == 0 ? "" : $"{string.Join("\n", notes.Select(n => n.ThisSummary))}";
        }
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