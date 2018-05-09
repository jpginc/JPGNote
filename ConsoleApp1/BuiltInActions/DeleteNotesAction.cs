using System;

namespace ConsoleApp1.BuiltInActions
{
    internal class DeleteNotesAction : SimpleTreeViewChoice
    {
        public DeleteNotesAction() : base("Delete notes")
        {
            AcceptHandler = DeleteNotes;
        }

        private void DeleteNotes(UserActionResult obj)
        {
            Console.WriteLine("Deleting all the notes!");
        }
    }
}