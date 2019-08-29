namespace ConsoleApp1
{
    internal class SelectingTriggersAcceptAction : SimpleTreeViewChoice
    {
        public SelectingTriggersAcceptAction(string choiceText, MainWindow window) : base(choiceText)
        {
            SelectHandler = window.Accept;
        }
    }
}