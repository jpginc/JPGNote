using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    [KnownType(typeof(Target))]
    internal class TargetManager : IManager
    {
        [IgnoreDataMember] private readonly ProjectSettingsClass _settings;

        [IgnoreDataMember] public string ManageText => "Manage Targets";
        [IgnoreDataMember] public string CreateChoiceText => "Create Target";

        [IgnoreDataMember] public string DeleteChoiceText => "Delete Targets";

        [DataMember] public List<ICreatable> Creatables { get; set; } = new List<ICreatable>();
        [IgnoreDataMember] public static TargetManager Instance { get; set; }

        public TargetManager(ProjectSettingsClass settings)
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
            Save();
        }

        public void New(UserActionResult obj)
        {
            //todo wizard 
            throw new NotImplementedException();
        }
    }

    [DataContract]
    internal class Target : ICreatable
    {
        [DataMember]
        [AutoSingleLineString]
        [Wizard]
        public string Name { get; set; }

        [DataMember]
        [AutoSingleLineString]
        [Wizard]
        public string IpOrDomain { get; set; }

        [IgnoreDataMember] public string EditChoiceText => Name;
    }
}