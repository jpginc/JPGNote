using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class ProgramProjectSetting : BaseCreatable
    {
        [DataMember, AutoSingleLineString, Wizard]
        public string ProjectName { get; set; }
        //[DataMember, Wizard, AutoFolderPicker]
        //public string ProjectFolder { get; set; }

        [IgnoreDataMember] public override string EditChoiceText => ProjectName;
        [IgnoreDataMember] public override List<string> ChildrenReferences => new List<string>();
        [IgnoreDataMember] public override string ThisSummary => "";
        [IgnoreDataMember] public IManager Manager => ProjectManager.Instance;
    }
}