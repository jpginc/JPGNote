using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class UserAction
    {
        public UserAction()
        {

        }

        [DataMember, AutoSingleLineString]
        public string Name { get; set; }
    }
}