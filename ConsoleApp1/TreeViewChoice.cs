using System;
using static ConsoleApp1.JoshSort;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal interface ITreeViewChoice : IComparable<ITreeViewChoice>
    {
        string GetChoiceText();
        ITreeViewChoice CalculateScore(string s);
        int GetScore();
        bool IsSelected();
        ITreeViewChoice SetSelected(bool selected);
    }

    internal class TreeViewChoice : ITreeViewChoice
    {
        public bool Selected = false;
        private readonly string _choiceText;
        private int _score = 0;

        public TreeViewChoice(string choiceText)
        {
            _choiceText = choiceText;
        }

        public string GetChoiceText()
        {
            return _choiceText;
        }

        public int GetScore()
        {
            return _score;
        }

        public ITreeViewChoice CalculateScore(string compareString)
        {
            _score = GetJoshScore(_choiceText, compareString);
            return this;
        }

        public int CompareTo(ITreeViewChoice other)
        {
            return other.GetScore().CompareTo(_score);
        }

        public bool IsSelected()
        {
            return Selected;
        }

        public ITreeViewChoice SetSelected(bool selected)
        {
            Selected = selected;
            return this;
        }
    }
}