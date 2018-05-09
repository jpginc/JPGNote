using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;

namespace ConsoleApp1
{
    public class JpgTreeView : TreeView
    {
        private readonly TreeStore _store = new TreeStore(typeof(string), typeof(ITreeViewChoice));
        private DateTime _lastClick = DateTime.Now;
        private ITreeViewChoice _lastValue;
        private readonly SearchEntry _search;

        private enum Column
        {
            Text,
            Value
        }

        public JpgTreeView SetMultiSelect(bool doMulti)
        {
            Selection.Mode = doMulti ? SelectionMode.Multiple : SelectionMode.Single;
            return this;
        }

        public JpgTreeView(SearchEntry search)
        {
            Model = _store;
            HeadersVisible = false;
            SearchEntry = new SearchEntry();
            //hooking up the search makes it select stuff 
            _search = search; 
            var valueColumn = new TreeViewColumn();
            AppendColumn(valueColumn);
            var visisbleColumnTextRenderer = new CellRendererText();
            valueColumn.PackStart(visisbleColumnTextRenderer, true);
            valueColumn.AddAttribute(visisbleColumnTextRenderer, "text", 0);
            ActivateOnSingleClick = true;
            RowActivated += ClickHandler;
        }

        private ITreeViewChoice GetValueFromIter(TreeIter item)
        {
            return (ITreeViewChoice) _store.GetValue(item, (int) Column.Value);
        }

        private bool CheckForDoubleClickOrDoubleReturn(TreeIter item)
        {
            var value = GetValueFromIter(item);
            var retVal = (DateTime.Now - _lastClick).Milliseconds < 200 && Equals(_lastValue, value);
            _lastClick = DateTime.Now;
            _lastValue = value;
            return retVal;
        }

        private void ClickHandler(object o, RowActivatedArgs args)
        {
            _store.GetIter(out var item, args.Path);
            if (CheckForDoubleClickOrDoubleReturn(item))
            {
                SetSelected(item, true);
                MainWindow.Instance.Accept();
            }
            else
            {
                Console.WriteLine("Clicked");
                SetSelected(item, true);
                _search.GrabFocusWithoutSelecting();
            }
        }

        private void SetSelected(TreeIter item, bool selected)
        {
            if (selected)
            {
                Selection.SelectIter(item);
                NotifyOfSelect(item);
            }
            else
            {
                Selection.UnselectIter(item);
            }
        }


        private void ToggleSelect(TreeIter item)
        {
            SetSelected(item, !Selection.IterIsSelected(item));
        }


        public JpgTreeView SetChoices(IEnumerable<ITreeViewChoice> choices)
        {
            _store.Clear();
            lock (choices)
            {
                foreach (var choice in choices)
                {
                    _store.AppendValues(choice.Text, choice);
                }
            }

            return this;
        }

        public JpgTreeView HandleSearchReturnKey()
        {
            _store.GetIterFirst(out var item);
            if (CheckForDoubleClickOrDoubleReturn(item))
            {
                SetSelected(item, true);
                MainWindow.Instance.Accept();
            }
            else
            {
                ToggleSelect(item);
            }

            return this;
        }

        private void NotifyOfSelect(TreeIter item)
        {
            ((ITreeViewChoice) _store.GetValue(item, (int) Column.Value))?
                .OnTreeViewSelectCallback(this);
        }

        public IEnumerable<ITreeViewChoice> GetSelectedItems()
        {
            IEnumerable<ITreeViewChoice> retVal = null;
            retVal = Selection.GetSelectedRows().Select(p =>
            {
                _store.GetIter(out var item, p);
                return (ITreeViewChoice) _store.GetValue(item, (int) Column.Value);
            }).ToList();
            return retVal;
        }

        public void UpdateOrder(string text)
        {
            //todo this is broken works good enough
            var startAgain = true;
            while (startAgain)
            {
                startAgain = false;
                var sortedRows = JoshSort.Sort(text, GetAllItemsWrappedWithTheRow()).ToArray();
                _store.GetIterFirst(out var iter);
                for (var i = 0; i < _store.IterNChildren(); i++)
                {
                    if (!sortedRows[i].Iter.Equals(iter))
                    {
                        _store.MoveBefore(sortedRows[i].Iter, iter);
                        startAgain = true;
                        break;
                    }
                    _store.IterNext(ref iter);
                }
            }
        }

        private IEnumerable<SortableRowWithValue> GetAllItemsWrappedWithTheRow()
        {
            var retVal = new List<SortableRowWithValue>();
            _store.GetIterFirst(out var iter);
            for (var i = 0; i < _store.IterNChildren(); i++)
            {
                retVal.Add(new SortableRowWithValue(iter, GetValueFromIter(iter)));
                _store.IterNext(ref iter);
            }

            return retVal;
        }

        public void RotateItems(bool forwardDirection)
        {
            Console.WriteLine("trying to rotate");
            _store.GetIterFirst(out var firstRow);
            var lastRow = GetLastRow();
            if (!forwardDirection)
                _store.MoveAfter(firstRow, lastRow);
            else
                _store.MoveBefore(lastRow, firstRow);
        }

        private TreeIter GetLastRow()
        {
            _store.GetIterFirst(out var iter);
            for (var i = 0; i < _store.IterNChildren() - 1; i++) _store.IterNext(ref iter);
            return iter;
        }
    }
}