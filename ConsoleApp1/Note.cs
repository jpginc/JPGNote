using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using ConsoleApp1.BuiltInActions;

namespace ConsoleApp1
{
    [DataContract]
    [KnownType(typeof(TcpPort))]
    [KnownType(typeof(UdpPort))]
    [KnownType(typeof(GlobalTcpPort))]
    [KnownType(typeof(GlobalUdpPort))]
    [KnownType(typeof(NewTarget))]
    [KnownType(typeof(NewUserNote))]
    [KnownType(typeof(NewLoggedNote))]
    [KnownType(typeof(GlobalUserNote))]
    [KnownType(typeof(NewProject))]
    [KnownType(typeof(Tag))]
    public abstract class BaseNote : INote, IComparable<INote>
    {
        [DataMember] public virtual List<string> TagsUniqueIds { get; } = new List<string>();
        [DataMember] public virtual string UniqueId { get; } = Guid.NewGuid().ToString("N");
        [DataMember] public List<string> ChildrenUniqueIds { get; } = new List<string>();
        [DataMember] public virtual string ParentUniqueId { get; set; }
        [DataMember] public string CreateTime { get; } = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        [DataMember, AutoSingleLineString, Wizard] public virtual string Name { get; set; }
        [DataMember, AutoMultiLineString, Wizard] public virtual string Contents { get; set; }
        public int CompareTo(INote other) => string.Compare(UniqueId, other.UniqueId, StringComparison.Ordinal);
    }

    [DataContract]
    public class NewUserNote : BaseNote
    {

    }

    [DataContract]
    public class NewLoggedNote : BaseNote
    {
        [DataMember] public override string Name { get; set; } = 
            $"Logged session {DateTime.Now:ddd MMM d yy h:m:sstt}";

        public NewLoggedNote(string fileName)
        {
            FileName = fileName;
        }
        [DataMember] public string FileName { get; set; }
        [IgnoreDataMember] public override string Contents
        {
            get => LoadLog();
            set { }
        }
        private string LoadLog()
        {
            string contents;
            try
            {

                var logFileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var logFileReader = new StreamReader(logFileStream);
                contents = logFileReader.ReadToEnd();
                logFileReader.Close();
                logFileStream.Close();
            }
            catch (Exception e)
            {
                contents = "Error opening log file";
            }
            return contents;
        }
    }

    [DataContract]
    public class GlobalUserNote : NewUserNote
    {
        [IgnoreDataMember] public override string ParentUniqueId => NewNotesManager.GetGlobalId();
    }

    [DataContract]
    public class NewProject : BaseNote
    {
        [IgnoreDataMember] public override string ParentUniqueId => NewNotesManager.GetGlobalId();
    }

    [DataContract]
    public class NewTarget : BaseNote
    {
        [DataMember, AutoSingleLineString, Wizard] public string TargetAddress { get; set; }
        [IgnoreDataMember] public override string Contents => TargetAddress;
    }
    public interface ITaggable
    {
        List<string> TagsUniqueIds { get; }
    }
    public interface INote : ITaggable
    {
        string UniqueId { get;  }
        string ParentUniqueId { get;  }
        List<string> ChildrenUniqueIds { get; }
        string Name { get; }
    }

    [DataContract]
    public class Tag : BaseNote
    {
        [IgnoreDataMember] public override string Contents => Name;
    }

    [DataContract]
    public abstract class Port : BaseNote
    {
        public abstract string PortType { get; }
        [DataMember, AutoSingleLineString, Wizard] public string PortNumber { get; set; }
        [IgnoreDataMember] public override string Contents => PortNumber;
        [IgnoreDataMember] public override string Name => $"{PortType} {PortNumber}";
    }

    [DataContract]
    public class TcpPort : Port
    {
        [IgnoreDataMember] public override string PortType => 
            $"{NewNotesManager.GetNote(ParentUniqueId)?.Name ?? "INVALID "} TcpPort ";
    }

    [DataContract]
    public abstract class GlobalPort : Port
    {
        [IgnoreDataMember] public override string UniqueId => PortNumber;
        [IgnoreDataMember] public override string ParentUniqueId => NewNotesManager.GetGlobalId();
    }

    [DataContract]
    public class UdpPort : Port
    {
        [IgnoreDataMember] public override string PortType => "UdpPort ";
    }

    [DataContract]
    public class GlobalTcpPort : GlobalPort
    {
        [IgnoreDataMember] public override string PortType => "Global TcpPort ";

    }

    [DataContract]
    public class GlobalUdpPort : GlobalPort
    {
        [IgnoreDataMember] public override string PortType => "Global TcpPort ";
    }

    [DataContract]
    public class Note : ICreatable, IComparable<Note>
    {
        [IgnoreDataMember] public string EditChoiceText => NoteName;
        [DataMember, AutoSingleLineString, Wizard] public string NoteName { get; set; }
        [DataMember] public readonly string UniqueId = Guid.NewGuid().ToString("N");
        [DataMember, AutoMultiLineString, Wizard] public virtual string NoteContents { get; set; } = "";
        [DataMember] public DateTime CreateTime { get; set; } = DateTime.Now;
        [DataMember] public List<string> Tags { get; set; } = new List<string>();
        public virtual int CompareTo(Note obj)
        {
            return string.Compare(UniqueId, obj.UniqueId, StringComparison.Ordinal);
        }
    }

    class UserNote : Note
    {
    }
}