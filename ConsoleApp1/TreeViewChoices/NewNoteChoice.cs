namespace ConsoleApp1.BuiltInActions
{
    internal class NewNoteChoice : SimpleTreeViewChoice
    {
        public NewNoteChoice() : base("New Note")
        {
            AcceptHandler = NotesManager.Instance.NewNoteAction;
        }
    }

}