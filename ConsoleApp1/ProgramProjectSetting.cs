using System;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class ProgramProjectSetting : ICreatable
    {
        [DataMember, AutoSingleLineString, Wizard]
        public string ProjectName { get; set; }
        [DataMember, Wizard, AutoFolderPicker]
        public string ProjectFolder { get; set; }

        public string EditChoiceText => ProjectName;
        public IManager Manager => ProjectManager.Instance;
    }
}