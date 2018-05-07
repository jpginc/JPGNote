using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class UserActionResult
    {
        public enum ResultType
        {
            Accept,
            Canceled,
            NoInput,
            ExitApp
        }

        public IEnumerable<ITreeViewChoice> UserChoices;
        public ResultType Result;
    }
}