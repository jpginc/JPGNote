using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;

namespace ConsoleApp1
{
    public class JpgTreeView : TreeView
    {
        private readonly TreeStore _store = new TreeStore(typeof(string), typeof(int), typeof(ITreeViewChoice));
        private DateTime _lastClick = DateTime.Now;
        private string _lastText;
        private TreeModelSort _sortedModel;
        private ITreeViewChoice _lastValue;
        private readonly SearchEntry _search;

        private enum Column
        {
            Text,
            SortValue,
            Value
        }

        public JpgTreeView SetMultiSelect(bool doMulti)
        {
            Selection.Mode = doMulti ? SelectionMode.Multiple : SelectionMode.Single;
            return this;
        }

        public JpgTreeView(SearchEntry search)
        {
            //Model = _store;
            HeadersVisible = false;
            SearchEntry = new SearchEntry();
            //hooking up the search makes it select stuff 
            _search = search; 
            var valueColumn = new TreeViewColumn();
            AppendColumn(valueColumn);
            var visisbleColumnTextRenderer = new CellRendererText();
            valueColumn.PackStart(visisbleColumnTextRenderer, true);
            valueColumn.AddAttribute(visisbleColumnTextRenderer, "text", (int) Column.Text);

            var sortingColumn = new TreeViewColumn
            {
                SortColumnId = (int) Column.SortValue,
            };
            AppendColumn(sortingColumn);
            var sortColumnRenderer = new CellRendererText();
            sortingColumn.PackStart(sortColumnRenderer, false);
            sortingColumn.AddAttribute(sortColumnRenderer, "text", (int)Column.SortValue);
            
            _sortedModel = new TreeModelSort(_store);
            _sortedModel.SetSortColumnId((int) Column.SortValue, SortType.Descending);
            _sortedModel.SetSortFunc((int) Column.SortValue, (model, a, b) =>
            {
                var aval = (int) model.GetValue(a, (int) Column.SortValue);
                var bval = (int) model.GetValue(b, (int) Column.SortValue);
                return aval.CompareTo(bval);
            });

            Model = _sortedModel;


            ActivateOnSingleClick = true;
            RowActivated += ClickHandler;
        }

        private ITreeViewChoice GetValueFromIter(TreeIter item)
        {
            return (ITreeViewChoice) _sortedModel.GetValue(item, (int) Column.Value);
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
            _sortedModel.GetIter(out var item, args.Path);
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
            //SetSelected(item, true);
            //SetSelected(item, !Selection.IterIsSelected(item));
        }


        public JpgTreeView SetChoices(IEnumerable<ITreeViewChoice> choices)
        {
            Console.WriteLine("setting a new choice list");
            _store.Clear();
            foreach (var choice in choices)
            {
                _store.AppendValues(choice.Text, 0, choice);
            }

            return this;
        }

        public JpgTreeView HandleSearchReturnKey()
        {
            var item = GetFirstRowItem();
            //_sortedModel.GetIterFirst(out var item);
            Console.WriteLine("return key give me " + GetValueFromIter(item)?.Text);
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

        private TreeIter GetFirstRowItem()
        {
            var x = _sortedModel.GetIterFirst(out var iter);
            Console.WriteLine("GetIterFirst returened " + x);

            return iter;
        }

        private void NotifyOfSelect(TreeIter item)
        {
            GetValueFromIter(item)?.OnTreeViewSelectCallback(this);
        }

        public IEnumerable<ITreeViewChoice> GetSelectedItems()
        {
            IEnumerable<ITreeViewChoice> retVal = null;
            retVal = Selection.GetSelectedRows().Select(p =>
            {
                _sortedModel.GetIter(out var item, p);
                return GetValueFromIter(item);
            }).ToList();
            return retVal;
        }

        public void UpdateOrder(string searchText)
        {
            _sortedModel.GetIterFirst(out var iter);
            var temp = GetValueFromIter(iter);
            Console.WriteLine("the first item i'm updating is " + temp?.Text);
            for (var i = 0; i < _sortedModel.IterNChildren(); i++)
            {
                ITreeViewChoice item = GetValueFromIter(iter);
                _store.SetValue(iter, (int) Column.SortValue, JoshSort.GetJoshScore(item.Text, searchText));
                _sortedModel.IterNext(ref iter);
            }
        }

        private IEnumerable<SortableRowWithValue> GetAllItemsWrappedWithTheRow()
        {
            var retVal = new List<SortableRowWithValue>();
            _sortedModel.GetIterFirst(out var iter);
            for (var i = 0; i < _sortedModel.IterNChildren(); i++)
            {
                retVal.Add(new SortableRowWithValue(iter, GetValueFromIter(iter)));
                _sortedModel.IterNext(ref iter);
            }

            return retVal;
        }

        public void RotateItems(bool forwardDirection)
        {
            Console.WriteLine("trying to rotate");
            var first = GetFirstRowItem();
            var last = GetLastRow();
            //if (forwardDirection)
            //{
                int firstVal = (int) _sortedModel.GetValue(first, (int) Column.SortValue);
                Console.WriteLine("First value is " + firstVal);
                Console.WriteLine("the iter is valid? " + _sortedModel.IterIsValid(last));
                Console.WriteLine("the iter is valid? " + _store.IterIsValid(last));
                _store.SetValue(last, (int) Column.SortValue, firstVal - 1);
            /*}
            else
            {
                var lastVal = (int) _sortedModel.GetValue(last, (int) Column.SortValue);
                Console.WriteLine("last value is " + lastVal);
                _sortedModel.Model.SetValue(first, (int) Column.SortValue, lastVal + 1);
            }
            */

        }

        private TreeIter GetLastRow()
        {
            _sortedModel.GetIterFirst(out var iter);
            for (var i = 0; i < _sortedModel.IterNChildren() - 1; i++) _sortedModel.IterNext(ref iter);
            return iter;
        }
    }
}