using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    public interface IManager 
    {
        string ManageText { get; }
        string CreateChoiceText { get; }
        string DeleteChoiceText { get; }
        List<ICreatable> Creatables { get; set; }
        void Save();
        void Delete(ICreatable creatable);
        void New(UserActionResult obj);
        bool HasChildren();
        IEnumerable<ICreatable> GetChildren(ICreatable parent);
    }

    public interface ICreatable 
    {
        string EditChoiceText { get; }
        string UniqueId { get; }
        List<string> ChildrenReferences { get; }
        string ThisSummary { get; }
        string FullSummary { get; }
        string SummaryForParent { get;}
    }

    [DataContract]
    public abstract class BaseCreatable : ICreatable
    {
        public abstract string EditChoiceText { get; }
        public abstract string ThisSummary { get; }

        [DataMember] public virtual string UniqueId { get; set; } = 
            Guid.NewGuid().ToString("N");
        [DataMember] public virtual List<string> ChildrenReferences { get; set; } = 
            new List<string>();
        [IgnoreDataMember] public virtual string FullSummary => ThisSummary;
        [IgnoreDataMember] public virtual string SummaryForParent => ThisSummary;
    }
}