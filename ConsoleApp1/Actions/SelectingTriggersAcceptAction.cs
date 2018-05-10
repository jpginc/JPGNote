namespace ConsoleApp1
{
    internal class SelectingTriggersAcceptAction : SimpleTreeViewChoice
    {
        public SelectingTriggersAcceptAction(string choiceText) : base(choiceText)
        {
            SelectHandler = MainWindow.Instance.Accept;
        }
    }
}