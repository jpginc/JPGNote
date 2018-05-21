using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ConsoleApp1.BuiltInActions;

namespace ConsoleApp1
{
    [DataContract]
    public class NotesStore
    {
        [DataMember] public List<INote> Notes { get; set; } = new List<INote>();
        [IgnoreDataMember] public static NotesStore Instance { get; set; } = new NotesStore();
    }
    public class NewNotesManager
    {
        private const string GlobalIdValue = "Global";
        private static IEnumerable<INote> Notes => NotesStore.Instance.Notes;

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
    }
}