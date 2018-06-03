using System;
using System.Collections.Generic;

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
}