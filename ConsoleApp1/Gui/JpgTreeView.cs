using System;
using System.Collections.Generic;
using System.Linq;
using Gdk;
using Gtk;

namespace ConsoleApp1
{
    public class JpgTreeView : TreeView
    {
        private const int DoubleClickInterval = 200;
        private readonly TreeStore _store = new TreeStore(typeof(string), typeof(int), 
            typeof(ITreeViewChoice));
        private DateTime _lastClick = DateTime.Now;
        private TreeModelSort _sortedModel;
        private ITreeViewChoice _lastClickedItem;
        private readonly SearchEntry _search;

        public Func<bool> AcceptCallback = DefaultAcceptCallback;

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
            _search = search;
            HeadersVisible = false;
            RemoveDefaultSearchPopup();
            SetupVisibleColumn();
            //SetupTheSortingColumn();
            SetupHandlers();
        }

        private static bool DefaultAcceptCallback() => true;

        private void SetupTheSortingColumn()
        {
            var sortingColumn = new TreeViewColumn
            {
                SortColumnId = (int) Column.SortValue
            };
            AppendColumn(sortingColumn);
            var sortColumnRenderer = new CellRendererText();
            sortingColumn.PackStart(sortColumnRenderer, false);
            sortingColumn.AddAttribute(sortColumnRenderer, "text", (int) Column.SortValue);
        }

        private void SetupHandlers()
        {
            ActivateOnSingleClick = true;
            KeyReleaseEvent += HandleKeyPresses;
            RowActivated += ClickHandler;
        }

        private void HandleDownUpEdgeCase(object o, KeyPressEventArgs args)
        {
        }

        protected override bool OnKeyPressEvent(EventKey evnt)
        {
            if (evnt.Key == Gdk.Key.Left)
            {
                SetItemAsStickySelected(false);
            }
            if (evnt.Key == Gdk.Key.Right)
            {
                SetItemAsStickySelected(true);
            }
            if (evnt.State != ModifierType.ControlMask 
                && (evnt.Key == Gdk.Key.Down || evnt.Key == Gdk.Key.Up))
            {
                //when you deselect a row with the left key you don't want to select it again
                GetCursor(out var path, out var column);
                if (! Selection.PathIsSelected(path))
                {
                    //move to the next item and select it, don't really know why this works...
                    SetCursor(path, column, false); 
                }
            }
 
            return base.OnKeyPressEvent(evnt);
        }

        private void SetItemAsStickySelected(bool makeSticky)
        {
            if (Selection.Mode == SelectionMode.Multiple)
            {
                GetCursor(out var path, out var column);
                _sortedModel.GetIter(out var row, path);
                var val = GetValueFromSortedRow(row);
                val.IsSticky = makeSticky;
                if (!makeSticky)
                {
                    Selection.UnselectPath(path);
                }
            }
        }

        public void HandleKeyPresses(object o, KeyReleaseEventArgs args)
        {
            var evnt = args.Event;
            if (evnt.Key == Gdk.Key.Down || evnt.Key == Gdk.Key.Up
                || evnt.Key == Gdk.Key.Next || evnt.Key == Gdk.Key.Prior)
            {
                foreach (ITreeViewChoice item in GetSelectedItems())
                {
                    item.OnTreeViewSelectCallback(this);
                }

                ReSelectStickyRows();
            }
            

            if (evnt.Key == Gdk.Key.d && args.Event.State == Gdk.ModifierType.Mod1Mask)
            {
                HandleDone();
            }

        }

        private void ReSelectStickyRows()
        {
            _sortedModel.GetIterFirst(out var iter);
            for (var i = 0; i < _sortedModel.IterNChildren() - 1; i++)
            {
                var value = GetValueFromSortedRow(iter);
                if (value.IsSticky)
                {
                    Selection.SelectIter(iter);
                }

                _sortedModel.IterNext(ref iter);
            }
        }

        private TreeIter GetSortedRowFromStoreRow(TreeIter iter)
        {
            var val = GetValueFromUnsortedRow(iter);
            _sortedModel.GetIterFirst(out var sortedIter);
            for (var i = 0; i < _sortedModel.IterNChildren() - 1; i++)
            {
                if (val == GetValueFromSortedRow(iter))
                {
                    return iter;
                }
                _sortedModel.IterNext(ref sortedIter);
            }
            throw new Exception("where did my row go?");
        }

        public void HandleDone()
        {
            foreach (ITreeViewChoice item in GetSelectedItems())
            {
                item.OnTreeViewDoneCallback(this);
            }
        }

        private void SetupVisibleColumn()
        {
            var valueColumn = new TreeViewColumn();
            AppendColumn(valueColumn);
            var visisbleColumnTextRenderer = new CellRendererText();
            valueColumn.PackStart(visisbleColumnTextRenderer, true);
            valueColumn.AddAttribute(visisbleColumnTextRenderer, "text", (int) Column.Text);
            SetupSortingFunction();
        }

        private void RemoveDefaultSearchPopup()
        {
            SearchEntry = new SearchEntry();
        }

        private void SetupSortingFunction()
        {
            _sortedModel = new TreeModelSort(_store);
            _sortedModel.SetSortColumnId((int) Column.SortValue, SortType.Descending);
            _sortedModel.SetSortFunc((int) Column.SortValue, (model, a, b) =>
            {
                var aval = (int) model.GetValue(a, (int) Column.SortValue);
                var bval = (int) model.GetValue(b, (int) Column.SortValue);
                return aval.CompareTo(bval);
            });
            Model = _sortedModel;
        }

        private ITreeViewChoice GetValueFromSortedRow(TreeIter item)
        {
            return (ITreeViewChoice) _sortedModel.GetValue(item, (int) Column.Value);
        }

        private bool CheckForDoubleClickOrDoubleReturn(TreeIter sortedRow)
        {
            var clickedItem = GetValueFromSortedRow(sortedRow);
            var isDoubleClick = (DateTime.Now - _lastClick).Milliseconds < DoubleClickInterval
                                && Equals(_lastClickedItem, clickedItem);
            _lastClick = DateTime.Now;
            _lastClickedItem = clickedItem;
            return isDoubleClick;
        }

        private void ClickHandler(object o, RowActivatedArgs args)
        {
            _sortedModel.GetIter(out var clickedRow, args.Path);
            if (CheckForDoubleClickOrDoubleReturn(clickedRow))
            {
                SelectSortedRow(clickedRow, true);
                AcceptCallback();
            }
            else
            {
                SelectSortedRow(clickedRow, true);
                //_search.GrabFocusWithoutSelecting();
            }
        }



        private void SelectSortedRow(TreeIter item, bool selected)
        {
            if (selected)
            {
                Selection.SelectIter(item);
                NotifyOfSortedRowSelect(item);
            }
            else
            {
                Selection.UnselectIter(item);
            }
        }


        private void ToggleSelectSortedRow(TreeIter sortedRow)
        {
            SelectSortedRow(sortedRow, !Selection.IterIsSelected(sortedRow));
        }


        public JpgTreeView SetChoices(IEnumerable<ITreeViewChoice> choices)
        {
            _store.Clear();
            foreach (var choice in choices) _store.AppendValues(choice.Text, 0, choice);
            return this;
        }

        public JpgTreeView HandleSearchReturnKey()
        {
            var sortedFirstRow = GetSortedFirstRow();
            //Console.WriteLine("return key give me " + GetValueFromSortedRow(sortedFirstRow)?.Text);
            if (CheckForDoubleClickOrDoubleReturn(sortedFirstRow))
            {
                SelectSortedRow(sortedFirstRow, true);
                AcceptCallback();
            }
            else
            {
                ToggleSelectSortedRow(sortedFirstRow);
            }

            return this;
        }

        private TreeIter GetSortedFirstRow()
        {
            var x = _sortedModel.GetIterFirst(out var iter);
            //Console.WriteLine("GetIterFirst returened " + x);
            return iter;
        }

        private TreeIter GetUnsortedFirstRow()
        {
            var x = _store.GetIterFirst(out var iter);
            //Console.WriteLine("GetIterFirst returened " + x);
            return iter;
        }


        private void NotifyOfSortedRowSelect(TreeIter sortedRow)
        {
            GetValueFromSortedRow(sortedRow)?.OnTreeViewSelectCallback(this);
        }

        public IEnumerable<ITreeViewChoice> GetSelectedItems()
        {
            IEnumerable<ITreeViewChoice> retVal = null;
            retVal = Selection.GetSelectedRows().Select(p =>
            {
                _sortedModel.GetIter(out var item, p);
                return GetValueFromSortedRow(item);
            }).ToList();
            return retVal;
        }

        public void UpdateOrder(string searchText)
        {
            if (searchText.Equals("*"))
            {
                SortByTextColumn();
                return;
            }
            var unsortedRow = GetUnsortedFirstRow();
            //var temp = GetValueFromSortedRow(unsortedRow);
            //Console.WriteLine("the first sortedRow i'm updating is " + temp?.Text);
            for (var i = 0; i < _store.IterNChildren(); i++)
            {
                var item = GetValueFromUnsortedRow(unsortedRow);
                _store.SetValue(unsortedRow, (int) Column.SortValue, JoshSort.GetJoshScore(item.Text, searchText));
                _store.IterNext(ref unsortedRow);
            }
        }

        private void SortByTextColumn()
        {
            List<ITreeViewChoice> allItems = new List<ITreeViewChoice>();
            _store.GetIterFirst(out var row);
            for (var i = 0; i < _store.IterNChildren(); i++)
            {
                allItems.Add(GetValueFromUnsortedRow(row));
                _store.IterNext(ref row);
            }
            SetChoices(allItems.OrderBy(i => i.Text));
        }

        private ITreeViewChoice GetValueFromUnsortedRow(TreeIter item)
        {
            return (ITreeViewChoice) _store.GetValue(item, (int) Column.Value);
        }

        public void RotateItems(bool forwardDirection)
        {
            var firstValue = GetValueFromSortedRow(GetSortedFirstRow());
            var lastValue = GetValueFromSortedRow(GetLastSortedRow());
            var firstRowInStore = GetStoreRowFromValue(firstValue);
            var lastRowInStore = GetStoreRowFromValue(lastValue);
            if (forwardDirection)
            {
                var firstVal = (int) _store.GetValue(firstRowInStore, (int) Column.SortValue);
                _store.SetValue(lastRowInStore, (int) Column.SortValue, firstVal + 1);
            }
            else
            {
                var lastVal = (int) _store.GetValue(lastRowInStore, (int) Column.SortValue);
                _store.SetValue(firstRowInStore, (int) Column.SortValue, lastVal - 1);
            }
        }

        private TreeIter GetStoreRowFromValue(ITreeViewChoice value)
        {
            _store.GetIterFirst(out var iter);
            for (var i = 0; i < _store.IterNChildren() - 1; i++)
            {
                if (GetValueFromUnsortedRow(iter) == value) return iter;
                _store.IterNext(ref iter);
            }

            return iter;
        }

        private TreeIter GetLastSortedRow()
        {
            _sortedModel.GetIterFirst(out var iter);
            for (var i = 0; i < _sortedModel.IterNChildren() - 1; i++) _sortedModel.IterNext(ref iter);
            return iter;
        }

        public void SelectEverything()
        {
            _sortedModel.GetIterFirst(out var iter);
            for (var i = 0; i < _sortedModel.IterNChildren(); i++)
            {
                Selection.SelectIter(iter);
                _sortedModel.IterNext(ref iter);
            }
        }
    }
}