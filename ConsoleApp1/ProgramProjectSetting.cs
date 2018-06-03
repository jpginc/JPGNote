using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class ProgramProjectSetting : ICreatable
    {
        [DataMember, AutoSingleLineString, Wizard]
        public string ProjectName { get; set; }
        //[DataMember, Wizard, AutoFolderPicker]
        //public string ProjectFolder { get; set; }

        [IgnoreDataMember] public string EditChoiceText => ProjectName;
        [DataMember] public string UniqueId { get; set; } = Guid.NewGuid().ToString("N");
        [IgnoreDataMember] public List<string> ChildrenReferences => null;
        [IgnoreDataMember] public string CreatableSummary => "";
        [IgnoreDataMember] public string FullSummary => "";
        [IgnoreDataMember] public IManager Manager => ProjectManager.Instance;
    }
}