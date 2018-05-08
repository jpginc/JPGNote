using System;

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
            UserNotifier.Notify("Double-Click or hit the Accept button to view more Note options");
            return true;
        }

        public override bool OnAcceptCallback(UserActionResult choice)
        {
            //NotesManager.Instance.HandleNoteAccept(_n, choice);
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