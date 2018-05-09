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
        private string _lastText;

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
            //SearchEntry = search;
            var valueColumn = new TreeViewColumn();
            AppendColumn(valueColumn);
            var visisbleColumnTextRenderer = new CellRendererText();
            valueColumn.PackStart(visisbleColumnTextRenderer, true);
            valueColumn.AddAttribute(visisbleColumnTextRenderer, "text", 0);
            ActivateOnSingleClick = true;
            RowActivated += ClickHandler;
        }

        private JpgTreeView UpdateSelectedItem(TreeIter item)
        {
            //GetValueFromIter(item)?.SetSelected(Selection.IterIsSelected(item));
            return this;
        }

        private ITreeViewChoice GetValueFromIter(TreeIter item)
        {
            return (ITreeViewChoice) _store.GetValue(item, (int) Column.Value);
        }

        private bool CheckForDoubleClickOrDoubleReturn(TreeIter item)
        {
            var cellText = GetValueFromIter(item)?.Text;
            var retVal = (DateTime.Now - _lastClick).Milliseconds < 200 && Equals(_lastText, cellText);
            _lastClick = DateTime.Now;
            _lastText = cellText;
            return retVal;
        }

        private void ClickHandler(object o, RowActivatedArgs args)
        {
            _store.GetIter(out var item, args.Path);
            if (CheckForDoubleClickOrDoubleReturn(item))
            {
                MainWindow.Instance.Accept();
            }
            else
            {
                Console.WriteLine("Clicked");
                SetSelected(item, true);
                SearchEntry.GrabFocusWithoutSelecting();
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

            UpdateSelectedItem(item);
        }


        private void ToggleSelect(TreeIter item)
        {
            SetSelected(item, !Selection.IterIsSelected(item));
        }


        public JpgTreeView SetChoices(IEnumerable<ITreeViewChoice> choices)
        {
            _store.Clear();
            foreach (var choice in choices)
            {
                var x = _store.AppendValues(choice.Text, choice);
            }

            return this;
        }

        public JpgTreeView HandleSearchReturnKey()
        {
            _store.GetIterFirst(out var item);
            if (CheckForDoubleClickOrDoubleReturn(item))
                MainWindow.Instance.Accept();
            else
                ToggleSelect(item);

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
                var sortedRows = JoshSort.Sort(text, GetAllItemsWrappedWithTheRow());
                _store.GetIterFirst(out var firstRow);

                var sortableRowWithValues = sortedRows as SortableRowWithValue[] ?? sortedRows.ToArray();
                //put the new first row before the current first row. then put the rest of the
                //ordered items after the new first row
                var newFirstRow = sortableRowWithValues.First();
                //if(! firstRow.Equals(newFirstRow.Iter))
                _store.MoveBefore(newFirstRow.Iter, firstRow);

                var currentRow = newFirstRow.Iter;
                //Console.WriteLine("1: " + newFirstRow.SortByText);
                foreach (var row in sortableRowWithValues.Skip(1))
                {
                    //Console.WriteLine(row.SortByText + " " + GetValueFromIter(row.Iter).SortByText);
                    _store.MoveAfter(currentRow, row.Iter);
                    currentRow = row.Iter;
                }

            //Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
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