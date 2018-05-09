using System.Collections.Generic;

namespace ConsoleApp1
{
    public interface IActionContext
    {
        IEnumerable<ITreeViewChoice> GetChoices();
    }
}