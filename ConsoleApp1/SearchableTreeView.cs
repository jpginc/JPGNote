using System;
using System.Collections.Generic;
using System.Linq;
using Gdk;
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
        private DateTime _lastRotate = DateTime.Now;

        public SearchableTreeView()
        {
            _label = new Label("Search: ");
            _search = new NoTabSearchEntry(this)
            {
                Expand = false,
                PlaceholderText = "<ctrl+i> to focus"
            };
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
            _treeview.HandleSearchReturnKey();
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
            GuiThread.Go(() =>
            {
                _choices = treeViewChoices.Select(s => s);
                UpdateChoices();
            });
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

        public SearchableTreeView Reset()
        {
            _search.Text = "";
            _treeview.SetMultiSelect(false);
            SetChoices(Enumerable.Empty<ITreeViewChoice>());
            return this;
        }

        public void FocusInput()
        {
            _search.GrabFocus();
        }

        public void HandleRotateKeypress(EventKey evnt)
        {
            if (evnt.Key == Gdk.Key.Down || evnt.Key == Gdk.Key.Up)
            {
                RoatateAndUpdateChoices(evnt.Key == Gdk.Key.Down);
            }
            else
            {
                RoatateAndUpdateChoices(evnt.State == ModifierType.ShiftMask);
            }
        }

        private void RoatateAndUpdateChoices(bool forwardDirection)
        {
            if ((DateTime.Now - _lastRotate).Milliseconds < 100) return;
            if (_choices.Count() <= 1)
                return;

            _choices = !forwardDirection
                ? _choices.Skip(1).Concat(_choices.Take(1))
                : _choices.TakeLast(1).Concat(_choices.SkipLast(1));
            UpdateChoices();
            _lastRotate = DateTime.Now;
        }
    }
}