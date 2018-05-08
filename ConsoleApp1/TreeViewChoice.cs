using System;
using static ConsoleApp1.JoshSort;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal abstract class TreeViewChoice : ITreeViewChoice
    {
        public bool Selected = false;
        private readonly string _choiceText;
        private int _score = 0;

        protected TreeViewChoice(string choiceText)
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

        //this could be made better but have to worry about threads with the treeview sort wrapper
        public bool IsSelected()
        {
            return Selected;
        }

        //this could be made better but have to worry about threads with the treeview sort wrapper
        public ITreeViewChoice SetSelected(bool selected)
        {
            if (selected)
            {
                Console.WriteLine("Selecting " + _choiceText);
            }
            Selected = selected;
            return this;
        }

        public abstract bool OnTreeViewSelectCallback(JpgTreeView jpgTreeView);


        public abstract bool OnAcceptCallback();
    }
}