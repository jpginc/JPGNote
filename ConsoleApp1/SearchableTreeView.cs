using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;

namespace ConsoleApp1
{
    internal class SearchableTreeView : Grid
    {
        private readonly Label _label;
        private readonly NoTabSearchEntry _search;
        private readonly JpgTreeView _treeview;
        private IEnumerable<ITreeViewChoice> _choices;
        public new int Height = 5;
        public new int Width = 1;

        public SearchableTreeView()
        {
            _label = new Label("Search: ");
            _search = new NoTabSearchEntry {Expand = false};
            _search.Changed += OnSearchChange;
            _search.Activated += OnSearchSubmit;

            BottomElement = new ScrolledWindow
            {
                ShadowType = ShadowType.EtchedIn,
                Expand = true
            };
            _treeview = new JpgTreeView(_search);
            BottomElement.Add(_treeview);
            Add(_label);
            AttachNextTo(_search, _label, PositionType.Bottom, Width, 1);
            AttachNextTo(BottomElement, _search, PositionType.Bottom, Width, Height);
        }

        private void OnSearchSubmit(object sender, EventArgs e)
        {
            _treeview.ToggleTopItem();
        }

        private void OnSearchChange(object sender, EventArgs e)
        {
            var searchText = ((SearchEntry) sender).Text;
            _choices = _choices.Select(s => s.CalculateScore(searchText)).OrderBy(s => s);
            UpdateChoices();
        }

        public SearchableTreeView SetLabelText(string text)
        {
            _label.Text = text;
            return this;
        }

        public ScrolledWindow BottomElement { get; }

        public void SetChoices(IEnumerable<ITreeViewChoice> treeViewChoices)
        {
            _choices = treeViewChoices.Select(s => s);
            UpdateChoices();
        }

        private void UpdateChoices()
        {
            _treeview.SetChoices(_choices);
        }

        public SearchableTreeView SetMultiSelect(bool b)
        {
            _treeview.SetMultiSelect(b);
            return this;
        }

        public IEnumerable<ITreeViewChoice> GetSelectedItems()
        {
            return _treeview.GetSelectedItems();
        }

        public string GetSearchValue()
        {
            return _search.Text;
        }
    }
}