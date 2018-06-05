using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ConsoleApp1.BuiltInActions;

namespace ConsoleApp1
{
    [DataContract]
    public class Tag : BaseCreatable, IComparable<Tag>
    {
        [DataMember, AutoSingleLineString, Wizard] public string TagName { get; set; }
        public override string EditChoiceText => TagName;
        public override string ThisSummary => TagName;
        public override string FullSummary => $"Tag: {TagName}\n{ChildSummaries}\n";

        public string ChildSummaries => string.Join("\n",
            ChildrenReferences.Select(c => ProgramSettingsClass.Instance.GetCreatable(c))
                .Where(c => c != null)
                .Select(c => c.ThisSummary));

        public int CompareTo(Tag obj)
        {
            return string.Compare(UniqueId, obj.UniqueId, StringComparison.Ordinal);
        }

        [IgnoreDataMember] public string TaggedItems
        {
            get
            {
                var tags = string.Join(", ", RefsToCreatablesThatAreTaggedByMe
                    .Select(r => ProgramSettingsClass.Instance.GetCreatable(r))
                    .Where(c => c != null)
                    .Select(c => c.GetType().Name + " " + c.EditChoiceText));
                return tags.Equals("") ? "" : $": {tags}";
            }
        }
        [DataMember] public List<string> RefsToCreatablesThatAreTaggedByMe { get; set; } = new List<string>();
        [DataMember] public List<string> RefsToNotesAboutThisTag { get; set; } = new List<string>();
    }
    [DataContract]
    public class Note : BaseCreatable, IComparable<Note>
    {
        [IgnoreDataMember] public override string EditChoiceText => NoteName;
        [DataMember] public List<string> TagReferences { get; set; } = new List<string>();
        [IgnoreDataMember] public override List<string> ChildrenReferences => new List<string>();
        public override string ThisSummary => NoteContents;
        [DataMember, AutoSingleLineString, Wizard] public string NoteName { get; set; }
        [DataMember, AutoMultiLineString, Wizard] public virtual string NoteContents { get; set; } = "";
        [DataMember] public DateTime CreateTime { get; set; } = DateTime.Now;
        [DataMember] public string ParentUniqueId { get; set; } = "";
        public virtual int CompareTo(Note obj)
        {
            return string.Compare(UniqueId, obj.UniqueId, StringComparison.Ordinal);
        }
    }

    class UserNote : Note
    {
    }
}