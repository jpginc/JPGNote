using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class BuiltInTypeChoice : SimpleTreeViewChoice, IActionProvider
    {
        private Type _type;
        private readonly bool _actionsToo;
        private readonly Action<IEnumerable<INote>> _action;

        public BuiltInTypeChoice(Type type, bool actionsToo = true, Action<IEnumerable<INote>> action = null) 
            : base($"Manage {type.Name}")
        {
            _type = type;
            _actionsToo = actionsToo;
            _action = action;
            AcceptHandler = (a) =>
            {
                JpgActionManager.PushActionContext(this);
            };
        }

        public InputType InputType => InputType.Multi;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            var c = new List<ITreeViewChoice>();
            c.AddRange(NewNotesManager.GetNotesByType(_type)
                .Select(n => new NoteChoice(n)));
            if (_actionsToo)
            {
                c.Add(new ActionChoice($"Delete {_type.Name}", DeleteNotes));
                c.Add(new ActionChoice($"New {_type.Name}", NewNote));
            }

            return c;
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            if (res.Result == UserActionResult.ResultType.Accept)
            {
                if (HasActionChoice(res.UserChoices))
                {
                    var action = GetAction(res.UserChoices);
                    if (HasNoteChoice(res.UserChoices))
                    {
                        action(res.UserChoices.Where(c => c.GetType() == typeof(NoteChoice))
                            .Select(c => ((NoteChoice) c).Note));
                    }
                    else
                    {
                        var x = new BuiltInTypeChoice(_type, false, action);
                        JpgActionManager.PushActionContext(x);
                    }
                }
                else
                {
                    IEnumerable<INote> newContextt = res.UserChoices.Select(c => ((NoteChoice) c).Note);
                    JpgActionManager.PushActionContext(newContextt);
                }
            }

            return ActionProviderResult.ProcessingFinished;
        }

        private Action<IEnumerable<INote>> GetAction(IEnumerable<ITreeViewChoice> choices)
        {
            return _action ?? (choices.First(c => c.GetType() == typeof(ActionChoice)) as ActionChoice)?.Action;
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