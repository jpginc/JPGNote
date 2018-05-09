using System;
using ConsoleApp1.BuiltInActions;

namespace ConsoleApp1
{
    internal class NoteTreeViewChoice : ITreeViewChoice
    {
        private readonly Note _note;
        public string SortByText => _note.NoteName;
        public string Text => _note.NoteName;

        public NoteTreeViewChoice(Note note)
        {
            _note = note;
        }

        public bool OnTreeViewSelectCallback(JpgTreeView jpgTreeView)
        {
            MainWindow.Instance.SetInputText(_note.NoteContents);
            return true;
        }

        public bool OnAcceptCallback(UserActionResult choice)
        {
            JpgActionManager.PushActionContext(new NoteAction(_note));
            MainWindow.Instance.SetInputText(_note.NoteContents);
            return true;
        }

        public bool OnSaveCallback(UserActionResult choice)
        {
            Console.WriteLine("the save data is " + choice.MultiLineInput);
            _note.NoteContents = choice.MultiLineInput;
            NotesManager.Instance.Save();
            return true;
        }

    }
}