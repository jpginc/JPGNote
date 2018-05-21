using System;
using System.Collections.Generic;

namespace ConsoleApp1.BuiltInActions
{
    internal class ActionChoice : SimpleTreeViewChoice
    {
        public readonly Action<IEnumerable<INote>> Action;

        public ActionChoice(string s, Action<IEnumerable<INote>> action) :base (s)
        {
            Action = action;
        }
    }
}