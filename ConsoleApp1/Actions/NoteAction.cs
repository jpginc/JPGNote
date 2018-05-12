using System.Collections.Generic;

namespace ConsoleApp1.BuiltInActions
{
    internal class NoteAction : SimpleActionProvider
    {
        private readonly INote _note;

        public NoteAction(INote note)
        {
            _note = note;
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
            NotesManager.Instance.Delete(_note);
            JpgActionManager.UnrollActionContext();
        }

        private void AddTag(UserActionResult obj)
        {
            var input = GuiManager.Instance.GetNonEmptySingleLineInput("Enter tag");
            if (input.ResponseType == UserActionResult.ResultType.Accept)
            {
                _note.Tags.Add(input.Result);
            }
        }



    }
}