using System;
using System.Collections.Generic;
using System.Linq;
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
    public class NotesStore
    {
        [DataMember] public List<INote> Notes { get; set; } = new List<INote>();
        [IgnoreDataMember] public static NotesStore Instance { get; set; } = new NotesStore();
        [IgnoreDataMember] public ISettingsClass Settings { get; set; }
        public void Save() => Settings.Save();
    }
    public class NewNotesManager
    {
        private const string GlobalIdValue = "Global";
        private static List<INote> Notes => NotesStore.Instance.Notes;
        public static void Save() => NotesStore.Instance.Save();

        public static INote GetNote(string noteUniqueId)
        {
            return Notes.FirstOrDefault(n => n.UniqueId.Equals(noteUniqueId));
        }

        public static string GetGlobalId()
        {
            return GlobalIdValue;
        }

        public static IEnumerable<INote> GetNotesByType(Type type)
        {
            return Notes.Where(n => n.GetType() == type);
        }

        public static void AddNote(INote note)
        {
            Notes.Add(note);
            Save();
        }

        public static IEnumerable<INote> GetChildren(INote note)
        {
            return note.ChildrenUniqueIds.Select(GetNote)
                .Where(n => n != null);
        }
    }
}