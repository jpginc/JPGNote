using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using ConsoleApp1.BuiltInActions;
using Gtk;

namespace ConsoleApp1
{
    [DataContract]
    internal class ProjectSettingsClass : ISettingsClass
    {
        private string _lock = "";
        private static string _fileName;
        private static string _password;
        private static readonly string _settingsFileName = "settings.txt";
        private static bool _stuffToSave = false;
        private static Timer _timer =new Timer(DoSave, null, 5000, Timeout.Infinite); 
        public static ProjectSettingsClass Instance { get; set; }

        [DataMember] public NotesManager NotesManager { get; private set; }
        [DataMember] public TargetManager TargetManager { get; private set; }
        [DataMember] public PortManager PortManager { get; private set; }

        public ProjectSettingsClass(NotesManager notesManager, TargetManager targetManager, 
            PortManager portManager)
        {
            NotesManager = notesManager;
            TargetManager = targetManager;
            PortManager = portManager;
        }
        private ProjectSettingsClass()
        {
            NotesManager = new NotesManager();
            TargetManager = new TargetManager();
            PortManager = new PortManager();
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
                //Console.WriteLine(s);
                file.Close();
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(s));
                var ser = new DataContractJsonSerializer(typeof(ProjectSettingsClass));
                Instance = ser.ReadObject(ms) as ProjectSettingsClass;
                ms.Close();
            }
            catch (FileNotFoundException e)
            {
                if (UserNotifier.Confirm("Project settings file not found, Create new file?")) Start();
            }
            catch (Exception e)
            {
                if (UserNotifier.Confirm("Something went wrong loading the file\n" +
                                         "Possibly wrong password. Would you like to trash" +
                                         "the old file and create a new project?"))
                {
                    file?.Close();
                    Start();
                }
            }
            finally
            {
                file?.Close();
            }

            NotesManager.Instance = Instance.NotesManager ?? new NotesManager();
            TargetManager.Instance = Instance.TargetManager ?? new TargetManager();
            TargetManager.Instance.Settings = Instance;
            PortManager.Instance = Instance.PortManager ?? new PortManager();
            PortManager.Instance.Settings = Instance;

            return Instance;
        }

        public void Save()
        {
            _stuffToSave = true;
        }

        private static void DoSave(object state)
        {
            Application.Invoke((a, b) =>
            {
                if (_stuffToSave)
                {
                    _stuffToSave = false;
                    var stream1 = new MemoryStream();
                    var ser = new DataContractJsonSerializer(ProjectSettingsClass.Instance.GetType());
                    ser.WriteObject(stream1, ProjectSettingsClass.Instance);
                    stream1.Position = 0;
                    var sr = new StreamReader(stream1);
                    //Console.Write("JSON form of Note object: ");
                    //Console.WriteLine(sr.ReadToEnd());
                    var writer = new StreamWriter(_fileName);
                    // Rewrite the entire value of s to the file
                    stream1.Position = 0;
                    writer.Write(AESThenHMAC.SimpleEncryptWithPassword(sr.ReadToEnd(), _password));
                    writer.Close();
                    Console.WriteLine("saved");
                }
                else
                {
                    Console.WriteLine("don't need to save");
                }
                _timer = new Timer(DoSave, null, 5000, Timeout.Infinite);
            });
        }

        public static ProjectSettingsClass Start()
        {
            Instance = new ProjectSettingsClass();
            Instance.Save();
            return Instance;
        }
    }
}