using System;
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

        public void NewNoteAction(UserActionResult userActionResult)
        {
            Console.WriteLine("the new not item has been selectd");
            var noteName = GuiManager.Instance.GetNonEmptySingleLineInput("Enter note name");
            if (noteName.Result != UserActionResult.ResultType.Canceled)
            {
                NewNote(noteName.SingleLineInput);
            }
        }

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

        public IEnumerable<NoteTreeViewChoice> GetNoteChoices()
        {
            return Notes.Select(n => new NoteTreeViewChoice(n));
        }

        public NotesManager Delete(Note note)
        {
            Notes.Remove(note);
            return this;
        }

        public NotesManager Save()
        {
            SettingsClass.Instance.Save();
            return this;
        }
    }
}