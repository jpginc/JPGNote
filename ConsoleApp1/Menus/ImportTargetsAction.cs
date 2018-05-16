using System.IO;

namespace ConsoleApp1.BuiltInActions
{
    internal class ImportTargetsAction : SimpleTreeViewChoice
    {
        private readonly Project _project;

        public ImportTargetsAction(Project project) : base("Import targets")
        {
            _project = project;
            AcceptHandler = GetTargetInput;
        }

        private void GetTargetInput(UserActionResult obj)
        {
            var input = GuiManager.Instance.GetMultiLineInput("Enter IP address, one per line", "");
            if (input.ResponseType == UserActionResult.ResultType.Accept)
            {
                var reader = new StringReader(input.Result);
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    _project.TargetManager.AddPremade(new Target {IpOrDomain = line});
                }
            }
        }
    }
}