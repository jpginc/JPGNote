using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class UserActionResult
    {
        public enum ResultType
        {
            Accept,
            Save,
            Canceled,
            NoInput,
            ExitApp
        }

        public IEnumerable<ITreeViewChoice> UserChoices;
        public string TreeViewSearchValue;
        public string InputBoxValue;
        public ResultType Result;
    }
}