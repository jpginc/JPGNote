using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ConsoleApp1
{
    [DataContract]
    internal class SettingsClass
    {
        private static string _fileName;
        public static SettingsClass Instance { get; set; }

        [DataMember] public NotesManager NotesManager { get; private set; }

        private SettingsClass()
        {
            NotesManager = new NotesManager();
        }

        public static SettingsClass Start(string fileName)
        {
            _fileName = fileName;
            try
            {
                var file = File.OpenText(fileName);
                var s = file.ReadToEnd();
                file.Close();
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(s));
                var ser = new DataContractJsonSerializer(typeof(SettingsClass));
                Instance = ser.ReadObject(ms) as SettingsClass;
                ms.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Instance = new SettingsClass();
            }

            NotesManager.Instance = Instance.NotesManager;

            return Instance;
        }

        public SettingsClass Save()
        {
            var stream1 = new MemoryStream();
            var ser = new DataContractJsonSerializer(this.GetType());
            ser.WriteObject(stream1, this);
            stream1.Position = 0;
            var sr = new StreamReader(stream1);
            Console.Write("JSON form of Note object: ");
            Console.WriteLine(sr.ReadToEnd());
            StreamWriter writer = new StreamWriter(_fileName);
            // Rewrite the entire value of s to the file
            stream1.Position = 0;
            writer.Write(sr.ReadToEnd());
            writer.Close();
            return this;
        }
    }
}