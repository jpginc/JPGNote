using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    [KnownType(typeof(SshAbleMachine))]
    [KnownType(typeof(Target))]
    [KnownType(typeof(UserAction))]
    [KnownType(typeof(ProgramProjectSetting))]
    [KnownType(typeof(Port))]
    internal abstract class Manager : IManager
    {
        public abstract string ManageText { get; }
        public abstract string CreateChoiceText { get; }
        public abstract string DeleteChoiceText { get; }
        [DataMember]
        public List<ICreatable> Creatables { get; set; } = new List<ICreatable>();
        public ISettingsClass Settings { get; set; }

        public void Save()
        {
            Settings.Save();
        }

        public void Delete(ICreatable creatable)
        {
            lock (Creatables)
            {
                Creatables.Remove(creatable);
            }

            Settings.Save();
        }

        public abstract void New(UserActionResult obj);
    }

    internal interface ISettingsClass
    {
        void Save();
    }
}