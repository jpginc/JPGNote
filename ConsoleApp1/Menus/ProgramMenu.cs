using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class ProgramMenu : SimpleActionProvider
    {
        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            /*var projects = new ManageableCreatable(ProjectManager.Instance).GetActionsWithChildren();
            var c = new List<ITreeViewChoice>
            {
                new ManageSlaveMachinesChoice(),
                new ManageUserActionsChoice(),
                new ExitChoice(),
            };
            return projects.Concat(c);
            */
            var menu = new GenericMenu();
            menu.Context = MenuContext.Program;
            menu.Notes = NewNotesManager.GetNotesByType(typeof(NewProject));
            return menu.GetChoices();
        }

    }

    public enum MenuContext
    {
        Program,
        Project,
        Target,
        Port
    }
    public class GenericMenu
    {
        public MenuContext Context { get; set; }
        public IEnumerable<INote> Notes { get; set; }

        public IEnumerable<ITreeViewChoice> GetChoices()
        {
            var choices = new List<ITreeViewChoice>();
           
            if (Context == MenuContext.Program)
            {
                choices.AddRange(new ManageableCreatable(MachineManager.Instance).GetActions());
                //choices.Add(new ManageUserActionsChoice());
                choices.Add(new ExitChoice());
            }

            return choices;
        }
    }
}