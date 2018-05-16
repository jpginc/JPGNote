using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1
{
    [DataContract]
    [KnownType(typeof(Note))]
    [KnownType(typeof(LoggedNote))]
    internal class NotesManager
    {
        public ProjectPersistence Settings { get; set; }
        public static NotesManager Instance { get; set; }
        [DataMember]
        public List<INote> Notes { get; set; }

        public void NewNoteAction(UserActionResult userActionResult)
        {
            Console.WriteLine("the new not item has been selectd");
            var noteName = GuiManager.Instance.GetNonEmptySingleLineInput("Enter note name");
            if (noteName.ResponseType != UserActionResult.ResultType.Canceled)
            {
                NewNote(noteName.Result);
            }
        }

        public NotesManager()
        {
            //don't want to change this because i'm doing serialisation stuff
            Notes = new List<INote>();
            Instance = this;
        }

        public NotesManager NewNote(string noteName)
        {
            var note = new Note(noteName);
            Notes.Add(note);
            return this;
        }

        public IEnumerable<NoteChoice> GetNoteChoices()
        {
            return Notes.Select(n => new NoteChoice(n));
        }

        public NotesManager Delete(INote note)
        {
            Notes.Remove(note);
            return this;
        }

        public NotesManager Save()
        {
            Settings.Save();
            return this;
        }

        public NotesManager NewLoggedNote(string fileName, string noteName)
        {
            Notes.Add(new LoggedNote(fileName, noteName));
            Save();
            return this;
        }
    }
}