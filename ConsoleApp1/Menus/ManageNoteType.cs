using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{

    internal class ManageNoteType : SimpleTreeViewChoice, IActionProvider
    {
        private Type _type;

        public ManageNoteType(Type type ) : base($"Manage {type.Name}")
        {
            _type = type;
            AcceptHandler = (a) =>
            {
                JpgActionManager.PushActionContext(this);
            };
        }

        public InputType InputType => InputType.Single;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return new List<ITreeViewChoice>
            {
                new SimpleTreeViewChoice($"New {_type.Name}"),
                new SimpleTreeViewChoice($"Delete {_type.Name}")
            };
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            if (res.Result == UserActionResult.ResultType.Accept)
            {
                if (HasActionChoice(res.UserChoices))
                {
                    JpgActionManager.PushActionContext(GetAction(res.UserChoices));
                }
                else
                {
                    IEnumerable<INote> newContextt = res.UserChoices.Select(c => ((NoteChoice) c).Note);
                    JpgActionManager.PushActionContext(newContextt);
                }
            }
            return ActionProviderResult.ProcessingFinished;
        }

        private IActionProvider GetAction(IEnumerable<ITreeViewChoice> choices)
        {
            return (choices.First(c => c.GetType() == typeof(ActionChoice)) as ActionChoice).Action;
        }

        private void NewNote(IEnumerable<INote> notes)
        {
            var note = (INote)Activator.CreateInstance(_type);
            if (CreatableWizard.GetRequiredFields(note))
            {
                NewNotesManager.AddNote(note);
            }
        }
        private void DeleteNotes(IEnumerable<INote> notes)
        {
            NewNotesManager.DeleteNotes(notes);
        }

        private bool HasNoteChoice(IEnumerable<ITreeViewChoice> choices)
        {
            return choices.Any(c => c.GetType() == typeof(NoteChoice));
        }

        private bool HasActionChoice(IEnumerable<ITreeViewChoice> choices)
        {
            return _actionsToo || choices.Any(c => c.GetType() == typeof(SimpleTreeViewChoice));
        }
    }
}