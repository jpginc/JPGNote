using System.Collections.Generic;

namespace ConsoleApp1.BuiltInActions
{
    internal class NewProjectChoice : SimpleTreeViewChoice
    {
        public NewProjectChoice() : base("New Project")
        {
            AcceptHandler = obj =>
            {
                var project = new NewProject();
                if (CreatableWizard.GetRequiredFields(project))
                {
                    NewNotesManager.AddNote(project);
                    JpgActionManager.PushActionContext(project);
                }
            };
        }
    }
}