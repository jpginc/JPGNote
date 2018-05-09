using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1
{
    public enum ActionProviderResult
    {
        ProcessingFinished,
        PassToTreeViewChoices
    }
    public interface IActionProvider
    {
        IEnumerable<ITreeViewChoice> GetActions();
        ActionProviderResult HandleUserAction(UserActionResult res);
    }

    [DataContract]
    abstract class SimpleActionProvider : IActionProvider
    {
        public abstract IEnumerable<ITreeViewChoice> GetActions();

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }
    }
}