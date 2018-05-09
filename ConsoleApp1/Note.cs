using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1
{
    [DataContract]
    internal class Note : IComparable<Note>, IActionContext
    {
        [DataMember] public readonly string NoteName;
        [DataMember] private readonly string _uniqueId;
        [DataMember] public string NoteContents { get; set; }
        [DataMember] public DateTime CreateTime { get; set; }
        [DataMember] public List<string> Tags { get; set; } = new List<string>();

        public Note(string noteName)
        {
            NoteName = noteName;
            _uniqueId = Guid.NewGuid().ToString("N");
            NoteContents = "";
            CreateTime = DateTime.Now;
        }

        public void ShowNoteAction(JpgTreeView treeView)
        {
            MainWindow.Instance.SetInputText(NoteContents);
        }

        public void SaveNoteAction(UserActionResult choice)
        {
            Console.WriteLine("the save data is " + choice.MultiLineInput);
            NoteContents = choice.MultiLineInput;
            NotesManager.Instance.Save();
        }

        public void ActivateNoteAction(UserActionResult choice)
        {
            JpgActionManager.ActionContext = this;
            MainWindow.Instance.SetInputText(NoteContents);
        }

        public int CompareTo(Note obj)
        {
            return string.Compare(_uniqueId, obj._uniqueId, StringComparison.Ordinal);
        }

        public IEnumerable<ITreeViewChoice> GetChoices()
        {
            return new[]
            {
                new TreeViewChoice("Delete note") {AcceptHandler = Delete},
                new TreeViewChoice("Add Tags") {AcceptHandler = AddTag}
            };
        }

        private void AddTag(UserActionResult obj)
        {
            var input = GuiManager.Instance.GetNonEmptySingleLineInput("Enter tag");
            if (input.Result == UserActionResult.ResultType.Accept)
            {
                Tags.Add(input.SingleLineInput);
            }
        }

        private void Delete(UserActionResult obj)
        {
            NotesManager.Instance.Delete(this);
            if (JpgActionManager.ActionContext == this)
            {
                JpgActionManager.ActionContext = null;
            }
        }

    }
}