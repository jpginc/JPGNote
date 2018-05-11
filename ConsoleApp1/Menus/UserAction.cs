using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class UserAction : ICreatable
    {
        public UserAction()
        {

        }

        [DataMember, AutoSingleLineString]
        public string Name { get; set; }

        [IgnoreDataMember]
        public string EditChoiceText => Name;
    }
}