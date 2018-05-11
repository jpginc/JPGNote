using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    internal abstract class Manager : IManager
    {
        public abstract string ManageText { get; }
        public abstract string CreateChoiceText { get; }
        public abstract string DeleteChoiceText { get; }
        public abstract List<ICreatable> Creatables { get; set; }
        private readonly ISettingsClass _settings;

        public Manager(ISettingsClass settings)
        {
            _settings = settings;
        }


        public void Save()
        {
            _settings.Save();
        }

        public void Delete(ICreatable creatable)
        {
            Creatables.Remove(creatable);
            _settings.Save();
        }

        public abstract void New(UserActionResult obj);
    }

    internal interface ISettingsClass
    {
        void Save();
    }
}