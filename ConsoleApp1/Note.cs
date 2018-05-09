using System;
using System.Runtime.Serialization;

namespace ConsoleApp1
{
    [DataContract]
    internal class Note : IComparable<Note>
    {
        [DataMember]
        public readonly string NoteName;
        [DataMember]
        private readonly string _uniqueId;
        [DataMember]
        public string NoteContents { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }

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
            SettingsClass.Instance.Save();
        }
        public int CompareTo(Note obj)
        {
            return string.Compare(_uniqueId, obj._uniqueId, StringComparison.Ordinal);
        }
    }
}