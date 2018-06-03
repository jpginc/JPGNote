using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class OutputFilterManager : Manager, IManagerAndActionProvider
    {
        [IgnoreDataMember] public static OutputFilterManager Instance { get; set; } = new OutputFilterManager();
        [IgnoreDataMember] public InputType InputType => InputType.Single;
        [IgnoreDataMember] public override string ManageText => "Manage Output Filters";
        [IgnoreDataMember] public override string CreateChoiceText => "New Output Filter";
        [IgnoreDataMember] public override string DeleteChoiceText => "Delete Output Filters";

        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return Creatables.Select(c => new AutoAction(c, this));
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }

        public override void New(UserActionResult obj)
        {
            New();
        }

        public Filter New()
        {
            var filter = new Filter();
            if (CreatableWizard.GetRequiredFields(filter))
            {
                Creatables.Add(filter);
                Console.WriteLine("Saving this thing");
                Save();
            }

            return filter;
        }
    }

    [DataContract]
    public class Filter : ICreatable
    {
        [IgnoreDataMember] public string EditChoiceText => $"Edit {Name}";

        [DataMember]
        [AutoSingleLineString]
        [Wizard]
        public string Name { get; set; } = "";
        [DataMember]
        [AutoSingleLineString]
        [Wizard]
        public string Regex { get; set; } = "";

        [DataMember] public string UniqueId { get; set; } = Guid.NewGuid().ToString("N");

        [IgnoreDataMember] public List<string> ChildrenReferences => new List<string>();

        [IgnoreDataMember] public string ThisSummary => $"{Name}: {Regex}";

        [IgnoreDataMember] public string FullSummary => ThisSummary;

        [IgnoreDataMember] public string SummaryForParent => ThisSummary;
    }
}