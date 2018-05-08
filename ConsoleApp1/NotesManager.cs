using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class NotesManager
    {
        public static NotesManager Instance { get; } = new NotesManager();
        public List<Note> Notes { get; }

        private NotesManager()
        {
            Notes = new List<Note>();
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