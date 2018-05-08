using System;

namespace ConsoleApp1
{
    internal class Note : IComparable<Note>
    {
        public readonly string NoteName;
        private readonly string _uniqueId;
        public string NoteContents { get; set; }

        public Note(string noteName)
        {
            NoteName = noteName;
            _uniqueId = Guid.NewGuid().ToString("N");
            NoteContents = "";
        }

        public int CompareTo(Note obj)
        {
            return string.Compare(_uniqueId, obj._uniqueId, StringComparison.Ordinal);
        }
    }
}