using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using ConsoleApp1;
using ConsoleApp1.BuiltInActions;

[DataContract]
internal class ProgramSettingsClass
{
    [IgnoreDataMember] private static string _fileName;
    [IgnoreDataMember] private static string _password;

    public ProgramSettingsClass()
    {
        ProjectManager = new ProjectManager();
        MachineManager = new MachineManager();
    }

    [IgnoreDataMember] public static ProgramSettingsClass Instance { get; set; }
    [DataMember] public ProjectManager ProjectManager { get; private set; }
    [DataMember] public MachineManager MachineManager { get; private set; }
    public string Password => _password;

    public static ProgramSettingsClass Start(string fileName, string password)
    {
        _password = password;
        StreamReader file = null;
        _fileName = fileName;
        try
        {
            file = File.OpenText(_fileName);
            var s = AESThenHMAC.SimpleDecryptWithPassword(file.ReadToEnd(), _password);
            Console.WriteLine(s);
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

        if (Instance.ProjectManager == null)
        {
            Instance.ProjectManager = new ProjectManager();
        }

        if (Instance.MachineManager == null)
        {
            Instance.MachineManager = new MachineManager();
        }

        ProjectManager.Instance = Instance.ProjectManager;
        MachineManager.Instance = Instance.MachineManager;

        return Instance;
    }

    public void Save()
    {
        var stream1 = new MemoryStream();
        var ser = new DataContractJsonSerializer(GetType());
        ser.WriteObject(stream1, this);
        stream1.Position = 0;
        var sr = new StreamReader(stream1);
        Console.Write("JSON form of Note object: ");
        Console.WriteLine(sr.ReadToEnd());
        var writer = new StreamWriter(_fileName);
        // Rewrite the entire value of s to the file
        stream1.Position = 0;
        writer.Write(AESThenHMAC.SimpleEncryptWithPassword(sr.ReadToEnd(), _password));
        writer.Close();
    }
}