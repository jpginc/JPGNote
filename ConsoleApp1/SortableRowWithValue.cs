using Gtk;

namespace ConsoleApp1
{
    public class SortableRowWithValue : IJoshSortable
    {
        public TreeIter Iter { get; set; }
        public ITreeViewChoice TreeViewChoice { get; set; }

        public SortableRowWithValue(TreeIter iter, ITreeViewChoice treeViewChoice)
        {
            Iter = iter;
            TreeViewChoice = treeViewChoice;
        }

        public string SortByText => TreeViewChoice.SortByText;
    }
}