using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ConsoleApp1.BuiltInActions;

namespace ConsoleApp1
{
    [DataContract]
    public class Tag : ICreatable, IComparable<Tag>
    {
        [DataMember, AutoSingleLineString, Wizard] public string TagName { get; set; }
        public string EditChoiceText => TagName;
        [DataMember] public string UniqueId { get; set; } = Guid.NewGuid().ToString("N");
        [IgnoreDataMember] public List<string> TagReferences => null;
        [DataMember] public List<string> NoteReferences { get; set; } = new List<string>();

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
    public class Note : ICreatable, IComparable<Note>
    {
        [IgnoreDataMember] public string EditChoiceText => NoteName;
        [DataMember] public List<string> TagReferences { get; set; } = new List<string>();
        [IgnoreDataMember] public List<string> NoteReferences => null;
        [DataMember, AutoSingleLineString, Wizard] public string NoteName { get; set; }
        [DataMember] public string UniqueId { get; set; } = Guid.NewGuid().ToString("N");
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