using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ConsoleApp1
{
    internal class NoteTreeViewChoice : TreeViewChoice
    {
        private readonly Note _n;

        public NoteTreeViewChoice(Note n) : base(n.NoteName)
        {
            _n = n;
        }

        public override bool OnTreeViewSelectCallback(JpgTreeView jpgTreeView)
        {
            MainWindow.Instance.SetInputText(_n.NoteContents); 
            return true;
        }

        public override bool OnAcceptCallback(UserActionResult choice)
        {
            return true;
        }

        public override bool OnSaveCallback(UserActionResult choice)
        {
            Console.WriteLine("the save data is " + choice.MultiLineInput);
            _n.NoteContents = choice.MultiLineInput;
            SettingsClass.Instance.Save();
            return true;
        }
    }
}