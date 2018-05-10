using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ConsoleApp1
{
    [DataContract]
    internal class ProjectSettingsClass
    {
        private static string _fileName;
        private static string _password;
        private static string _settingsFileName = "settings.txt";
        public static ProjectSettingsClass Instance { get; set; }

        [DataMember] public NotesManager NotesManager { get; private set; }

        private ProjectSettingsClass()
        {
            NotesManager = new NotesManager();
        }

        public static ProjectSettingsClass Load(string folderName, string password)
        {
            _password = password;
            StreamReader file = null;
            _fileName = folderName + Path.DirectorySeparatorChar + _settingsFileName;
            try
            {
                file = File.OpenText(_fileName);
                var s = AESThenHMAC.SimpleDecryptWithPassword(file.ReadToEnd(), _password);
                Console.WriteLine(s);
                file.Close();
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(s));
                var ser = new DataContractJsonSerializer(typeof(ProjectSettingsClass));
                Instance = ser.ReadObject(ms) as ProjectSettingsClass;
                ms.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                file?.Close();
            }

            NotesManager.Instance = Instance.NotesManager;

            return Instance;
        }

        public ProjectSettingsClass Save()
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
            writer.Write(AESThenHMAC.SimpleEncryptWithPassword(sr.ReadToEnd(), _password));
            writer.Close();
            return this;
        }

        public static ProjectSettingsClass Start(string folder, string password)
        {
            _fileName = folder + Path.DirectorySeparatorChar + _settingsFileName;
            _password = password;
            Instance = new ProjectSettingsClass();
            Instance.Save();
            return Instance;
        }
    }
}