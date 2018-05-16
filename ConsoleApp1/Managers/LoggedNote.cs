using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace ConsoleApp1
{
    [DataContract]
    internal class LoggedNote : INote
    {
        [DataMember] public string FileName { get; set; }
        [DataMember] public DateTime CreateTime { get; set; }

        [IgnoreDataMember] public string NoteContents
        {
            get => LoadLog();
            set { }
        }

        public string LoadLog()
        {
            string contents;
            try
            {

                var logFileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var logFileReader = new StreamReader(logFileStream);
                contents = logFileReader.ReadToEnd();
                logFileReader.Close();
                logFileStream.Close();
            }
            catch (Exception e)
            {
                contents = "Error opening log file";
            }
            return contents;
        }

        [DataMember]
        public string NoteName { get; set; }
        [DataMember] public List<string> Tags { get; set; }
        public LoggedNote(string fileName, string noteName)
        {
            FileName = fileName;
            NoteName = noteName;
            CreateTime = DateTime.Now;
        }
        public int CompareTo(object obj)
        {
            return obj.GetType() != this.GetType() ? -1
                : string.Compare(FileName, ((LoggedNote) obj).FileName, StringComparison.Ordinal);
        }
    }
}