using System.Collections.Generic;

namespace ConsoleApp1.BuiltInActions
{
    internal class NoteAction : SimpleActionProvider
    {
        private Note note;

        public NoteAction(Note note)
        {
            this.note = note;
        }

        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            return new[]
            {
                new SimpleTreeViewChoice("Delete note") {AcceptHandler = Delete},
                new SimpleTreeViewChoice("Add Tags") {AcceptHandler = AddTag}
            };
        }
        private void Delete(UserActionResult obj)
        {
            NotesManager.Instance.Delete(note);
            JpgActionManager.PushActionContext(this);
        }

        private void AddTag(UserActionResult obj)
        {
            var input = GuiManager.Instance.GetNonEmptySingleLineInput("Enter tag");
            if (input.Result == UserActionResult.ResultType.Accept)
            {
                note.Tags.Add(input.SingleLineInput);
            }
        }



    }
}