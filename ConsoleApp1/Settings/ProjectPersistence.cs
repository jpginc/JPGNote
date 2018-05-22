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
    public class ProjectPersistence : ISettingsClass
    {
        private const int SaveTimerInterval = 1000;
        private static bool _stuffToSave;
        private Timer _timer;
        private static string _password;

        private static readonly string _settingsFileName = "settings.txt";
        public string ProjectName;
        private bool _isSaving;
        [IgnoreDataMember] public Project Project { get; set; }

        private string SettingFileName => ProjectFolder + Path.DirectorySeparatorChar + ProjectName
                                          + Path.DirectorySeparatorChar + _settingsFileName;

        [DataMember] public NotesManager NotesManager { get; private set; }
        [DataMember] public TargetManager TargetManager { get; private set; }
        [DataMember] public PortManager PortManager { get; private set; }
        [DataMember] public TagManager TagManager { get; private set; }
        [DataMember] public CommandQueue CommandQueue { get; set; }

        public string ProjectFolder { get; private set; }

        public bool Create(string folderName, string projectName, string password)
        {
            _password = password;
            ProjectName = projectName;
            ProjectFolder = folderName;
            if (CreateProjectFolder(folderName + Path.DirectorySeparatorChar + projectName)
                && StartNew())
            {
                StartSaveTimer();
                return true;
            }

            return false;
        }

        private void StartSaveTimer()
        {
            _timer = new Timer(b => SaveAsync(), null, SaveTimerInterval, Timeout.Infinite);
        }

        public bool Load(string folderName, string projectName, string password)
        {
            _password = password;
            ProjectName = projectName;
            ProjectFolder = folderName;
            if (Load())
            {
                StartSaveTimer();
                return true;
            }

            return false;
        }


        private bool CreateProjectFolder(string folderName)
        {
            try
            {
                Directory.CreateDirectory(folderName);
                if (File.Exists(SettingFileName))
                {
                    if (UserNotifier.Confirm("Project already exists. Select Yes to load, No to overwrite"))
                    {
                        return true;
                    }

                    File.Delete(SettingFileName);
                    return true;
                }
                return true;
            }
            catch (IOException e)
            {
                UserNotifier.Error("Some problem with creating the project folder!");
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        private bool Load()
        {
            StreamReader file = null;
            try
            {
                file = File.OpenText(SettingFileName);
                var s = AESThenHMAC.SimpleDecryptWithPassword(file.ReadToEnd(), _password);
                //Console.WriteLine(s);
                file.Close();
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(s));
                var ser = new DataContractJsonSerializer(GetType());
                var instance = ser.ReadObject(ms) as ProjectPersistence;
                ms.Close();

                NotesManager = instance.NotesManager;
                NotesManager.Settings = this;
                TargetManager = instance.TargetManager;
                TargetManager.Settings = this;
                PortManager = instance.PortManager;
                PortManager.Settings = this;
                TagManager = instance.TagManager ?? new TagManager();
                TagManager.Settings = this;
                //must be last
                CommandQueue = instance.CommandQueue ?? new CommandQueue();
            }
            catch (FileNotFoundException e)
            {
                if (UserNotifier.Confirm("Project settings file not found, Create new file?")) StartNew();
            }
            catch (Exception e)
            {
                if (UserNotifier.Confirm("Something went wrong loading the file\n" +
                                         "Possibly wrong password. Would you like to trash" +
                                         "the old file and create a new project?"))
                {
                    file?.Close();
                    StartNew();
                }
            }
            finally
            {
                file?.Close();
            }

            return true;
        }

        public void Save()
        {
            _stuffToSave = true;
            StartSaveTimer();
        }


        private void SaveAsync(object o)
        {
            SaveAsync();
        }

        private void SaveAsync()
        {
            Application.Invoke((a, b) =>
            {
                if (_stuffToSave && !_isSaving)
                {
                    _isSaving = true;
                    _stuffToSave = false;
                    new Thread(() =>
                    {
                        Persist();
                        Application.Invoke((c,d) => 
                        {
                            _isSaving = false;
                            if (_stuffToSave)
                            {
                                StartSaveTimer();
                            }
                        });
                    }).Start();
                }
            });
        }

        private bool Persist()
        {
            try
            {
                var stream1 = new MemoryStream();
                var ser = new DataContractJsonSerializer(GetType());
                ser.WriteObject(stream1, this);
                stream1.Position = 0;
                var sr = new StreamReader(stream1);
                var writer = new StreamWriter(SettingFileName);
                // Rewrite the entire value of publics to the file
                stream1.Position = 0;
                writer.Write(AESThenHMAC.SimpleEncryptWithPassword(sr.ReadToEnd(), _password));
                writer.Close();
                //Console.WriteLine("saved");
                return true;
            }
            catch (Exception e)
            {
                //UserNotifier.Error("Error persisting \n" + e.StackTrace);
                Console.WriteLine("Error persisting \n" + e.StackTrace);
                _stuffToSave = true;
                return false;
            }
        }

        private bool StartNew()
        {
            NotesManager = new NotesManager {Settings = this};
            TargetManager = new TargetManager {Settings = this};
            PortManager = new PortManager {Settings = this};
            TagManager = new TagManager {Settings = this};
            CommandQueue = new CommandQueue();
            return Persist();
        }
    }
}