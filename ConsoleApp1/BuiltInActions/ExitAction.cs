using System;

namespace ConsoleApp1.BuiltInActions
{
    internal class ExitAction : SimpleTreeViewChoice
    {
        public ExitAction() : base("Exit")
        {
            AcceptHandler = (a) => Environment.Exit(0);
        }
    }
}