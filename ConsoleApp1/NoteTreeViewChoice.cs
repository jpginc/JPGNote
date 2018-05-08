namespace ConsoleApp1
{
    internal class NoteTreeViewChoice : TreeViewChoice
    {
        private readonly Note _n;

        public NoteTreeViewChoice(Note n) : base(n.NoteName)
        {
            _n = n;
        }

        public override bool OnTreeViewSelectCallback(JpgTreeView jpgTreeView)
        {
            MainWindow.Instance.SetInputText(_n.NoteContents); 
            return true;
        }

        public override bool OnAcceptCallback()
        {
            return true;
        }
    }
}