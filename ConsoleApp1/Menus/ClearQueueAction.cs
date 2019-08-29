namespace ConsoleApp1.BuiltInActions
{
    internal class ClearQueueAction : SimpleTreeViewChoice
    {
        private Project _project;

        public ClearQueueAction(Project project): base("Clear Command Queue")
        {
            _project = project;
            AcceptHandler = ClearQueue;
        }
        private void ClearQueue(UserActionResult obj)
        {
            //_project.CommandQueue.ClearQueue(_project);
        }
    }
}