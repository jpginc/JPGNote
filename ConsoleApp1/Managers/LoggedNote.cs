using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace ConsoleApp1
{
    [DataContract]
    internal class LoggedNote : INote
    {
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }

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
                contents = "";

                while (!logFileReader.EndOfStream)
                {
                    contents += logFileReader.ReadLine();
                    contents += Environment.NewLine;
                }

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
        [DataMember]
        public List<string> Tags { get; set; }
        public LoggedNote(string fileName)
        {
            FileName = fileName;
            NoteName = "Logged Session " + DateTime.Now.ToLongDateString();
            CreateTime = DateTime.Now;
        }
        public int CompareTo(object obj)
        {
            return obj.GetType() != this.GetType() ? -1
                : string.Compare(FileName, ((LoggedNote) obj).FileName, StringComparison.Ordinal);
        }
    }
}