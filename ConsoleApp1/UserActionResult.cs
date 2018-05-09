using System.Collections.Generic;

namespace ConsoleApp1
{
    public class UserActionResult
    {
        public enum ResultType
        {
            Accept,
            Save,
            Canceled,
            NoInput
        }

        public IEnumerable<ITreeViewChoice> UserChoices;
        public string SingleLineInput;
        public string MultiLineInput;
        public ResultType Result;
    }
}