namespace ConsoleApp1.BuiltInActions
{
    internal class CreateNoteAction : SimpleTreeViewChoice
    {
        public CreateNoteAction() : base("New Note")
        {
            AcceptHandler = NotesManager.Instance.NewNoteAction;
        }
    }

}