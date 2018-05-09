using System.Collections.Generic;

namespace ConsoleApp1
{
    public interface IActionProvider
    {
        IEnumerable<ITreeViewChoice> GetActions();
    }
}