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
        [IgnoreDataMember] public override string ManageText => "Manage MyTagsAndChildTags";
        [IgnoreDataMember] public override string CreateChoiceText => "New Tag";
        [IgnoreDataMember] public override string DeleteChoiceText => "Delete MyTagsAndChildTags";

        public override void New(UserActionResult obj)
        {
            New();
        }

        public Tag New()
        {
            var userNote = new Tag();
            if (CreatableWizard.GetRequiredFields(userNote))
            {
                Creatables.Add(userNote);
                Save();
            }

            return userNote;
        }
        public InputType InputType => InputType.Multi;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return Creatables.Select(c => new AutoAction(c, this));
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            if (res.Result == UserActionResult.ResultType.Accept)
            {
                var x = new SelectCommandToRunMenu(Settings.Project);
                x.PrepopulatePorts(res.UserChoices
                    .Select(c => ((AutoAction)c).Creatable as Tag)
                    .SelectMany(t => t.RefsToCreatablesThatAreTaggedByMe
                        .Select(r => ProgramSettingsClass.Instance.GetPort(r))
                        .Where(p => p != null)));
                JpgActionManager.PushActionContext(x);
                return ActionProviderResult.ProcessingFinished;
            }
            return ActionProviderResult.PassToTreeViewChoices;
        }

        public Tag GetTagByUniqueId(string uniqueId)
        {
            return Creatables.FirstOrDefault(n => ((Tag)n).UniqueId.Equals(uniqueId)) as Tag;
        }

        public void AddPremade(Tag tag)
        {
            Creatables.Add(tag);
        }

        public Tag GetOrCreateTag(string tagText)
        {
            var existing = Creatables.FirstOrDefault(c => ((Tag)c).TagName.Equals(tagText)) as Tag;
            if (existing != null)
            {
                return existing;
            }

            var newTag = new Tag() {TagName = tagText};
            Creatables.Add(newTag);
            return newTag;
        }

        public void CreateLinkedTag(ICreatable port)
        {
            var tag = New();
            port.ChildrenReferences.Add(tag.UniqueId);
            tag.RefsToCreatablesThatAreTaggedByMe.Add(port.UniqueId);
        }
    }
}