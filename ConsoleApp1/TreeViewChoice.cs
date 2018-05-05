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
    }

    internal class TreeViewChoice : ITreeViewChoice
    {
        public bool Selected = false;
        private string ChoiceText;
        private int Score = 0;

        public TreeViewChoice(string choiceText)
        {
            ChoiceText = choiceText;
        }

        public string GetChoiceText()
        {
            return ChoiceText;
        }

        public int GetScore()
        {
            return Score;
        }

        public ITreeViewChoice CalculateScore(string compareString)
        {
            Score = GetJoshScore(ChoiceText, compareString);
            return this;
        }

        public int CompareTo(ITreeViewChoice other)
        {
            return other.GetScore().CompareTo(Score);
        }

        public bool IsSelected()
        {
            return Selected;
        }

        public void Select(bool isSelected)
        {
            Selected = isSelected;
        }

        public void Select()
        {
            Selected = true;
        }

        public void DeSelect()
        {
            Selected = false;
        }
    }
}