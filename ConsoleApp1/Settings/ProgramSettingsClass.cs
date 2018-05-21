using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using ConsoleApp1;
using ConsoleApp1.BuiltInActions;
using Gtk;

[DataContract]
internal class ProgramSettingsClass : ISettingsClass
{
    [DataMember] public MachineManager MachineManager { get; private set; }
    [DataMember] public UserActionManager UserActionManager { get; private set; }
    [DataMember] public NotesStore NotesStore { get; private set; }
    [IgnoreDataMember] public string Password => _password;
    [IgnoreDataMember] public static string SettingFileName => FolderName 
               + Path.DirectorySeparatorChar + FileName;

    [IgnoreDataMember] public static ProgramSettingsClass Instance { get; set; }
    [IgnoreDataMember] public static string FileName;
    [IgnoreDataMember] public static string FolderName;
    [IgnoreDataMember] private static string _password;
    [IgnoreDataMember] private const int SaveTimerInterval = 1000;
    [IgnoreDataMember] private static bool _stuffToSave;
    [IgnoreDataMember] private Timer _timer;
    [IgnoreDataMember] private bool _isSaving;
 

    public ProgramSettingsClass()
    {
        MachineManager = new MachineManager() {Settings = this};
        UserActionManager = new UserActionManager() {Settings = this};
        NotesStore = new NotesStore() {Settings = this};
    }
    public void Save()
    {
        _stuffToSave = true;
        StartSaveTimer();
    }

    public static ProgramSettingsClass Start(string folderName, string fileName, string password)
    {
        FolderName = folderName;
        _password = password;
        FileName = fileName;
        StreamReader file = null;
        try
        {
            file = File.OpenText(SettingFileName);
            var s = AESThenHMAC.SimpleDecryptWithPassword(file.ReadToEnd(), _password);
            //Console.WriteLine(s);
            file.Close();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(s));
            var ser = new DataContractJsonSerializer(typeof(ProgramSettingsClass));
            Instance = ser.ReadObject(ms) as ProgramSettingsClass;
            ms.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            Instance = new ProgramSettingsClass();
            //throw e;
        }
        finally
        {
            file?.Close();
        }

        MachineManager.Instance = Instance.MachineManager ?? new MachineManager();
        MachineManager.Instance.Settings = Instance;
        UserActionManager.Instance = Instance.UserActionManager ?? new UserActionManager();
        UserActionManager.Instance.Settings = Instance;
        NotesStore.Instance = Instance.NotesStore ?? new NotesStore();
        NotesStore.Instance.Settings = Instance;

        return Instance;
    }

    private void StartSaveTimer()
    {
        _timer = new Timer(b => SaveAsync(), null, SaveTimerInterval, Timeout.Infinite);
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
}