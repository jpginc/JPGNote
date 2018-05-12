﻿using System;
using System.Collections.Generic;

namespace ConsoleApp1.BuiltInActions
{
    //todo split up the action and the choice
    internal class DeleteNotesAction : SimpleTreeViewChoice, IActionProvider
    {
        public InputType InputType => InputType.Multi;
        public DeleteNotesAction() : base("Delete notes")
        {
            AcceptHandler = GiveContext;
        }

        private void GiveContext(UserActionResult obj)
        {
            JpgActionManager.PushActionContext(this);
        }

        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return NotesManager.Instance.GetNoteChoices();
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            if (res.Result == UserActionResult.ResultType.Accept)
            {
                foreach (var treeViewChoice in res.UserChoices)
                {
                    var choice = (ConsoleApp1.NoteChoice) treeViewChoice;
                    NotesManager.Instance.Delete(choice.Note);
                }
                ProjectSettingsClass.Instance.Save();
            }
            JpgActionManager.UnrollActionContext();
            return ActionProviderResult.ProcessingFinished;
        }
    }
}