using System;

namespace ConsoleApp1
{

    internal interface ITreeViewChoice : IComparable<ITreeViewChoice>
    {
        //todo figure out sorted treeviews and remove the rubbish from this class
        string GetChoiceText();
        ITreeViewChoice CalculateScore(string s);
        int GetScore();
        bool IsSelected();
        ITreeViewChoice SetSelected(bool selected);
        bool OnTreeViewSelectCallback(JpgTreeView jpgTreeView);

        bool OnAcceptCallback(UserActionResult choice);
        bool OnSaveCallback(UserActionResult choice);
        Action<JpgTreeView> SelectHandler { get; set; }
        Action<UserActionResult> AcceptHandler { get; set; }
        Action<UserActionResult> SaveHandler { get; set; }
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
            _score = JoshSort.GetJoshScore(_choiceText, compareString);
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

        public bool OnTreeViewSelectCallback(JpgTreeView jpgTreeView)
        {
            SelectHandler(jpgTreeView);
            return true;
        }


        public bool OnAcceptCallback(UserActionResult choice)
        {
            AcceptHandler(choice);
            return true;
        }

        public bool OnSaveCallback(UserActionResult choice)
        {
            SaveHandler(choice);
            return true;
        }
        public Action<JpgTreeView> SelectHandler { get; set; } = DoNothingHandler;

        public Action<UserActionResult> AcceptHandler { get; set; } = DoNothingHandler;
        public Action<UserActionResult> SaveHandler { get; set; } = DoNothingHandler;

        private static void DoNothingHandler(object obj)
        {
        }
    }

}