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
        OutputFilterManager = new OutputFilterManager() {Settings = this};
    }

    [IgnoreDataMember] public static ProgramSettingsClass Instance { get; set; }
    [DataMember] public ProjectManager ProjectManager { get; set; }
    [DataMember] public MachineManager MachineManager { get; set; }
    [DataMember] public UserActionManager UserActionManager { get; set; }
    [DataMember] public OutputFilterManager OutputFilterManager { get; set; }
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
        OutputFilterManager.Instance = Instance.OutputFilterManager ?? new OutputFilterManager();
        OutputFilterManager.Instance.Settings = Instance;

        return Instance;
    }

    public void Save()
    {
        Console.WriteLine("Saving program settings");
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

    public Note GetNote(string uniqueNoteId)
    {
        foreach (var project in ProjectManager.LoadedProjects)
        {
            Note note = null;
            if ((note = project.NotesManager.GetNoteByUniqueId(uniqueNoteId)) != null)
            {
                return note;
            }
        }
        return null;
    }

    public Tag GetTag(string tagId)
    {
        foreach (var project in ProjectManager.LoadedProjects)
        {
            Tag tag = null;
            if ((tag = project.TagManager.GetTagByUniqueId(tagId)) != null)
            {
                return tag;
            }
        }
        return null;
    }

    public ICreatable GetCreatable(string uid)
    {
        //Console.WriteLine("looking for " + uid);
        ICreatable a = GetTag(uid);
        if (a != null) return a;
        a = GetNote(uid);
        if (a != null)
            return a;
        a = GetPort(uid);
        if (a != null)
            return a;
        a = GetTarget(uid);
        return a;

    }

    public Port GetPort(string uid)
    {
        foreach (var project in ProjectManager.LoadedProjects)
        {
            Port port = null;
            if ((port = project.PortManager.GetPortById(uid)) != null)
            {
                return port;
            }
        }
        return null;       
    }

    public ProjectPersistence GetProject(ICreatable creatable)
    {
        foreach (var project in ProjectManager.LoadedProjects)
        {
            if (project.Contains(creatable))
            {
                return project;
            }
        }
        return null;
    }

    public Target GetTarget(string uid)
    {
        foreach (var project in ProjectManager.LoadedProjects)
        {
            Target target = null;
            if ((target = project.TargetManager.GetTargetById(uid)) != null)
            {
                return target;
            }
        }
        return null;
    }
}