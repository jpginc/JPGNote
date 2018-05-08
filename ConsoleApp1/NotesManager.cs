using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1
{
    [DataContract]
    internal class NotesManager
    {
        public static NotesManager Instance { get; set; }
        [DataMember]
        public List<Note> Notes { get; set; }

        public NotesManager()
        {
            //don't want to change this because i'm doing serialisation stuff
            Notes = new List<Note>();
            Instance = this;
        }

        public NotesManager NewNote(string noteName)
        {
            var note = new Note(noteName);
            Notes.Add(note);
            return this;
        }


        public IEnumerable<ITreeViewChoice> GetNoteChoices()
        {
            return Notes.Select(n => new NoteTreeViewChoice(n));
        }
    }
}