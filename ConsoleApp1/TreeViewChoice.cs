using System;
using static ConsoleApp1.JoshSort;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal interface ITreeViewChoice : IComparable<ITreeViewChoice>
    {
        string GetChoiceText();

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

        public void CalculateScore(string compareString)
        {
            Score = GetJoshScore(ChoiceText, compareString);
        }

        public int CompareTo(ITreeViewChoice other)
        {
            return GetScore().CompareTo(other.GetScore());
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