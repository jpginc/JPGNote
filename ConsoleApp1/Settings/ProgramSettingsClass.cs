using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using ConsoleApp1;
using ConsoleApp1.BuiltInActions;

[DataContract]
internal class ProgramSettingsClass : ISettingsClass
{
    [IgnoreDataMember] public static string FileName;
    [IgnoreDataMember] private static string _password;
    private string _lock = "";
    [IgnoreDataMember] public static string FolderName;

    public ProgramSettingsClass()
    {
        ProjectManager = new ProjectManager() {Settings = this, LoadedProjects = new List<ProjectPersistence>() };
        MachineManager = new MachineManager();
        UserActionManager = new UserActionManager() {Settings = this};
    }

    [IgnoreDataMember] public static ProgramSettingsClass Instance { get; set; }
    [DataMember] public ProjectManager ProjectManager { get; private set; }
    [DataMember] public MachineManager MachineManager { get; private set; }
    [DataMember] public UserActionManager UserActionManager { get; private set; }
    public string Password => _password;

    public static ProgramSettingsClass Start(string folderName, string fileName, string password)
    {
        FolderName = folderName;
        _password = password;
        StreamReader file = null;
        FileName = folderName + Path.DirectorySeparatorChar + fileName;
        try
        {
            file = File.OpenText(FileName);
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

        ProjectManager.Instance = Instance.ProjectManager ?? new ProjectManager();
        ProjectManager.Instance.Settings = Instance;
        ProjectManager.Instance.LoadedProjects = new List<ProjectPersistence>();
        MachineManager.Instance = Instance.MachineManager ?? new MachineManager();
        UserActionManager.Instance = Instance.UserActionManager ?? new UserActionManager();
        UserActionManager.Instance.Settings = Instance;

        return Instance;
    }

    public void Save()
    {
            var stream1 = new MemoryStream();
            var ser = new DataContractJsonSerializer(GetType());
            ser.WriteObject(stream1, this);
            stream1.Position = 0;
            var sr = new StreamReader(stream1);
            //Console.Write("JSON form of Note object: ");
            //Console.WriteLine(sr.ReadToEnd());
            var writer = new StreamWriter(FileName);
            // Rewrite the entire value of s to the file
            stream1.Position = 0;
            writer.Write(AESThenHMAC.SimpleEncryptWithPassword(sr.ReadToEnd(), _password));
            writer.Close();
    }

    public Project Project { get; set; }

    public ICreatable GetNote(string uniqueNoteId)
    {
        foreach (var project in ProjectManager.LoadedProjects)
        {
            Note note = null;
            if ((note = project.NotesManager.GetNote(uniqueNoteId)) != null)
            {
                return note;
            }
        }
        return null;
    }
}