using System;

namespace ConsoleApp1
{
    internal class SimpleTreeViewChoice : TreeViewChoice
    {
        public override bool OnTreeViewSelectCallback(JpgTreeView jpgTreeView)
        {
            Console.WriteLine("this callback is working");
            return true;
        }

        public override bool OnAcceptCallback(UserActionResult choice)
        {
            return true;
        }

        public override bool OnSaveCallback(UserActionResult choice)
        {
            return true;
        }

        public SimpleTreeViewChoice(string choiceText) : base(choiceText)
        {
        }
    }
}