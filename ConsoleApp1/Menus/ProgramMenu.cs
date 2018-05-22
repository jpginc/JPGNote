﻿using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class ProgramMenu : SimpleActionProvider
    {
        private GenericMenu _genericMenu;

        public ProgramMenu()
        {
            _genericMenu = new GenericMenu();
            _genericMenu.Notes = NewNotesManager.GetNotesByType(typeof(NewProject));
        }

        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            return _genericMenu.GetChoices();
        }

    }

    public class GenericMenu
    {
        public INote Context => JpgActionManager.GetNoteContext();
        public IEnumerable<INote> Notes { get; set; }

        public IEnumerable<ITreeViewChoice> GetChoices()
        {
            var choices = new List<ITreeViewChoice>();

            if (Context == null)
            {
                choices.AddRange(NewNotesManager.GetNotesByType(typeof(NewProject))
                    .Select(n => new NoteChoice(n)));
                choices.Add(new NewProjectChoice());
            }

            choices.AddRange(new ManageableCreatable(MachineManager.Instance).GetActions());
            choices.AddRange(new ManageableCreatable(UserActionManager.Instance).GetActions());
            if (Context == null) 
            {
                choices.Add(new ExitChoice());
            }
            return choices;
        }
    }
}