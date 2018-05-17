using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ConsoleApp1.BuiltInActions;

namespace ConsoleApp1
{
    [DataContract]
    public class NotesManager : Manager, IManagerAndActionProvider
    {
        [IgnoreDataMember] public override string ManageText => "Manage Notes";
        [IgnoreDataMember] public override string CreateChoiceText => "New Note";
        [IgnoreDataMember] public override string DeleteChoiceText => "Delete Notes";
        //[DataMember] public List<INote> Notes { get; set; }

        public override void New(UserActionResult obj)
        {
            var userNote = new Note();
            if (CreatableWizard.GetRequiredFields(userNote))
            {
                Creatables.Add(userNote);
                Save();
            }
        }

        public LoggedNote NewLoggedNote(string fileName, string noteName)
        {
            var note = new LoggedNote(fileName, noteName);
            Creatables.Add(note);
            Save();
            return note;
        }

        public InputType InputType => InputType.Single;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return Creatables.Select(c => new AutoAction(c, this));
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }
    }
}