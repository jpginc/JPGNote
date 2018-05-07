using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class UserActionResult
    {
        public enum ResultType
        {
            Accept,
            Canceled,
            NoInput
        }

        public IEnumerable<ITreeViewChoice> UserChoices;
        public ResultType Result;
    }
}