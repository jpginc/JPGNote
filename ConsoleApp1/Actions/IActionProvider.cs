using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1
{
    public enum ActionProviderResult
    {
        ProcessingFinished,
        PassToTreeViewChoices
    }

    public enum InputType
    {
        Single,
        Multi
    }
    public interface IActionProvider
    {
        InputType InputType { get; }
        IEnumerable<ITreeViewChoice> GetActions();
        ActionProviderResult HandleUserAction(UserActionResult res);
    }

    public abstract class SimpleActionProvider : IActionProvider
    {
        public InputType InputType => InputType.Single;
        public abstract IEnumerable<ITreeViewChoice> GetActions();

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }
    }
}