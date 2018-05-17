using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    [KnownType(typeof(SshAbleMachine))]
    [KnownType(typeof(Target))]
    [KnownType(typeof(Note))]
    [KnownType(typeof(LoggedNote))]
    [KnownType(typeof(UserAction))]
    [KnownType(typeof(ProgramProjectSetting))]
    [KnownType(typeof(Port))]
    public abstract class Manager : IManager
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
        public virtual bool HasChildren()
        {
            return false;
        }

        public virtual IEnumerable<ICreatable> GetChildren(ICreatable parent)
        {
            return Enumerable.Empty<ICreatable>();
        }
    }

    public interface ISettingsClass
    {
        void Save();
        Project Project { get; set; }
    }
}