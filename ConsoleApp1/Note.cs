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
        /* TODO
        container
            -> UUID
            -> name (for command parsing)
            -> displayName (for listview)
            -> value (might be the name or might be the output of a command or manual text)
            -> previewText 
            -> fullText
            -> attributes[{name:value}]
            -> children[]
            -> parent
        there are containers that don't have values but contain things that have values and possibly other containersC.
        localhost
            -> contains a tag/note that says it's a target
            -> domain name -> value localhost
            -> ip address -> value 127.0.0.1
        

        allow multiple groups of things e.g.
        if we filter recursively e.g. 
            1. get all root types [targets1] [targets2] and [wordlist]
            2. filter the roots to ensure that the secont level root types exist
                a. remove any targets that don't have a domain name.
                b. remove any targets that don't have a port
                c. remove any wordlists that don't have a path
            3. from the filtered root types, get the next level of roots and filter.

        * target:domainName
            find containers that have are 'targets'
                find children of those that are 'domainName's
        * target:port:httpscheme
            find containers that have are 'targets'
                find children of those that are 'ports's
                    find children of those that are 'httpscheme's
        * target:port
            find containers that have are 'targets'
                find children of those that are 'ports's
        * wordlist:path?
            find containers that are 'wordlist's
                find children of those that are 'path's

        xss.vg      
        -> is a 'target'
        -> is a 'aws' 
        children [
            -> xss.vg 
            -> is a domainName
            -> 1.1.1.1
            -> is an ipAddress
            -> 22
            -> is a 'port'
                -> ssh -> true
            -> port -> 80
                -> http -> true
                -> scheme -> https
                -> wordpress -> true
            ]
        wordlists is a container
            -> contains a tag saying that it's a wordlist (so that it can be referenced in )
            -> contains children 
                -> a note saying the details of a single wordlist
                -> a note saying the details of a single wordlist
                -> a note saying the details of a single wordlist

        guid
        name e.g. localhost, 80, wordlist
        display name e.g. localhost, 80, all.txt
        preview text e.g. localhost, 80, ""
        full text e.g. localhost, 80, ""
        value e.g. localhost, 80, /home/user/wordlists/all.txt
        modified times [] this includes the create time.
        children [] e.g. [80, a note with the name target], [note with name 'port'], [note that says what command created this one]
        metadata children [] e.g. created by a command so that command can delete it, this could just be a note this is a child...
        */
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