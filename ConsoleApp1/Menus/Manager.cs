using System.Collections.Generic;

namespace ConsoleApp1.BuiltInActions
{
    internal class Manager : IManager
    {
        private IEnumerable<ICreatable _creatable;
        private readonly ISettingsClass _settings;

        public Manager(ICreatable creatable, ISettingsClass settings)
        {
            _creatable = creatable;
            _settings = settings;
        }

        public string ManageText => _creatable.CreateChoiceText;
        public bool AcceptCallback(UserActionResult choice)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<BuiltInActions.ICreatable> GetCreatables()
        {
            throw new System.NotImplementedException();
        }

        public void Save(ICreatable creatable)
        {
            
        }

        public void Delete(ICreatable creatable)
        {
            throw new System.NotImplementedException();
        }
    }

    internal interface ISettingsClass
    {
        void Save();
    }
}