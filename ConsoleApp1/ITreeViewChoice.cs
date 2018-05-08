using System;

namespace ConsoleApp1
{
    internal interface ITreeViewChoice : IComparable<ITreeViewChoice>
    {
        string GetChoiceText();
        ITreeViewChoice CalculateScore(string s);
        int GetScore();
        bool IsSelected();
        ITreeViewChoice SetSelected(bool selected);
        bool OnTreeViewSelectCallback(JpgTreeView jpgTreeView);
        bool OnAcceptCallback();

    }
}