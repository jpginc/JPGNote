using System;

namespace ConsoleApp1
{
    class NewNoteTreeViewChoice : TreeViewChoice
    {
        public NewNoteTreeViewChoice(string choiceText) : base(choiceText)
        {
        }

        public override bool OnTreeViewSelectCallback(JpgTreeView jpgTreeView)
        {
            return true;
        }

        public override bool OnAcceptCallback()
        {
            var noteName = GuiManager.Instance.GetNonEmptySingleLineInput("Enter note name");
            if (noteName.Result != UserActionResult.ResultType.Canceled)
            {
                NotesManager.Instance.NewNote(noteName.SingleLineInput);
            }
            return true;
        }
    }
}