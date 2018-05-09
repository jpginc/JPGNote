using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class Project 
    {
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string FileName { get; set; }
        public Project(string projectName)
        {
            ProjectName = projectName;
        }
    }
}