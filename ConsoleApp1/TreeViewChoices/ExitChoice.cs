using System;

namespace ConsoleApp1.BuiltInActions
{
    internal class ExitChoice : SimpleTreeViewChoice
    {
        public ExitChoice() : base("Exit")
        {
            AcceptHandler = (a) => Environment.Exit(0);
        }
    }
}