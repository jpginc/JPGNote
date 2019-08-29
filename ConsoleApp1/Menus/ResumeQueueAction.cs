namespace ConsoleApp1.BuiltInActions
{
    internal class ResumeQueueAction : SimpleTreeViewChoice
    {
        private Project _project;

        public ResumeQueueAction(Project project) : base("Resume Queue")
        {
            _project = project;
            AcceptHandler = ResumeQueue;
        }

        private void ResumeQueue(UserActionResult obj)
        {
            //_project.CommandQueue.Revive(_project);
        }
    }
}