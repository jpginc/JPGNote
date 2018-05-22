using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ConsoleApp1.BuiltInActions;

namespace ConsoleApp1
{
    [DataContract]
    [KnownType(typeof(Tag))]
    public class TagManager : Manager, IManagerAndActionProvider
    {
        [IgnoreDataMember] public override string ManageText => "Manage Tags";
        [IgnoreDataMember] public override string CreateChoiceText => "New Tag";
        [IgnoreDataMember] public override string DeleteChoiceText => "Delete Tags";

        public override void New(UserActionResult obj)
        {
            var userNote = new Tag();
            if (CreatableWizard.GetRequiredFields(userNote))
            {
                Creatables.Add(userNote);
                Save();
            }
        }
        public InputType InputType => InputType.Multi;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return Creatables.Select(c => new AutoAction(c, this));
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }

        public Tag GetTag(string uniqueId)
        {
            return Creatables.FirstOrDefault(n => ((Tag)n).UniqueId.Equals(uniqueId)) as Tag;
        }

        public void AddPremade(Tag tag)
        {
            Creatables.Add(tag);
        }
    }
}