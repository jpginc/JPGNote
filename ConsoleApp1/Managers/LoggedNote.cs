using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using ConsoleApp1.BuiltInActions;

namespace ConsoleApp1
{
    [DataContract]
    internal class LoggedNote : Note
    {
        [DataMember] public string FileName { get; set; }

        [IgnoreDataMember] public override string NoteContents
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

        public LoggedNote(string fileName, string noteName)
        {
            FileName = fileName;
            NoteName = noteName;
            CreateTime = DateTime.Now;
        }
    }
}