using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class ProgramProjectSetting 
    {
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string ProjectFolder { get; set; }
        public ProgramProjectSetting(string projectName, string projectFolder)
        {
            ProjectName = projectName;
            ProjectFolder = projectFolder;
        }
    }
}