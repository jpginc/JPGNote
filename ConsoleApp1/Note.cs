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