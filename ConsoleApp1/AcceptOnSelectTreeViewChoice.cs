using System;

namespace ConsoleApp1
{
    internal class AcceptOnSelectTreeViewChoice : TreeViewChoice
    {
        public AcceptOnSelectTreeViewChoice(string choiceText) : base(choiceText)
        {
        }

        public override bool OnTreeViewSelectCallback(JpgTreeView jpgTreeView)
        {
            Console.WriteLine("select callback working");
            MainWindow.Instance.Accept();
            return true;
        }

        public override bool OnAcceptCallback(UserActionResult choice)
        {
            throw new NotImplementedException();
        }

        public override bool OnSaveCallback(UserActionResult choice)
        {
            return true;
        }
    }
}