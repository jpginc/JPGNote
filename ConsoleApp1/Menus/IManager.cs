using System.Collections.Generic;

namespace ConsoleApp1.BuiltInActions
{
    public interface IManager
    {
        string ManageText { get; }
        IEnumerable<ICreatable> GetCreatables();
        void Save(ICreatable creatable);
        void Delete(ICreatable creatable);
    }

    public interface ICreatable
    {
        string EditChoiceText { get; }
        string CreateChoiceText { get; }
        string DeleteChoiceText { get; }
    }
}