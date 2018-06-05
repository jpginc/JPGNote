using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

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
                Save();
            }

            return filter;
        }

        internal string FilterString(string noteContents)
        {
            try
            {

                var regexs = Creatables.Where(f => !((Filter)f).Regex.Equals(""))
                        .Select(f => new Regex(((Filter)f).Regex));
                foreach (var regex in regexs)
                {
                    noteContents = regex.Replace(noteContents, "");
                }
            } catch(Exception e)
            {

            }
            return noteContents;
        }
    }

    [DataContract]
    public class Filter : BaseCreatable
    {
        [IgnoreDataMember] public override string EditChoiceText => $"Edit {Name}";

        [DataMember]
        [AutoSingleLineString]
        [Wizard]
        public string Name { get; set; } = "";
        [DataMember]
        [AutoSingleLineString]
        [Wizard]
        public string Regex { get; set; } = "";

        [IgnoreDataMember] public override string ThisSummary => $"{Name}: {Regex}";
    }
}