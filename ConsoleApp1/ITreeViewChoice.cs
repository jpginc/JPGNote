using System;

namespace ConsoleApp1
{
    public interface ITreeViewChoice : IJoshSortable
    {
        //todo figure out sorted treeviews and remove the rubbish from this class
        string Text { get; }
        bool OnTreeViewSelectCallback(JpgTreeView jpgTreeView);
        bool OnAcceptCallback(UserActionResult choice);
        bool OnSaveCallback(UserActionResult choice);
    }
    internal class SimpleTreeViewChoice : ITreeViewChoice
    {

        public SimpleTreeViewChoice(string choiceText)
        {
            Text = choiceText;
        }

        public string Text { get; set; }

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

        public string SortByText => Text;
    }

}