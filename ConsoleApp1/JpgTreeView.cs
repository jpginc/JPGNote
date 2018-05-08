using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;

namespace ConsoleApp1
{
    internal class JpgTreeView : TreeView
    {
        private readonly ListStore _store = new ListStore(typeof(string), typeof(int), typeof(ITreeViewChoice));
        private DateTime _lastClick = DateTime.Now;
        private string _lastText;
        private TreeModelSort _sortedModel;

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
            SearchEntry = search;
            var valueColumn = new TreeViewColumn();
            AppendColumn(valueColumn);
            var visisbleColumnTextRenderer = new CellRendererText();
            valueColumn.PackStart(visisbleColumnTextRenderer, true);
            valueColumn.AddAttribute(visisbleColumnTextRenderer, "text", (int) Column.Text);

            var sortingColumn = new TreeViewColumn
            {
                SortColumnId = (int) Column.SortValue,
                Visible = false
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
            return (ITreeViewChoice) _store.GetValue(item, (int) Column.Value);
        }

        private bool CheckForDoubleClickOrDoubleReturn(TreeIter item)
        {
            var cellText = GetValueFromIter(item)?.GetChoiceText();
            var retVal = (DateTime.Now - _lastClick).Milliseconds < 200 && Equals(_lastText, cellText);
            _lastClick = DateTime.Now;
            _lastText = cellText;
            return retVal;
        }

        private void ClickHandler(object o, RowActivatedArgs args)
        {
            _store.GetIter(out var item, args.Path);
            if(CheckForDoubleClickOrDoubleReturn(item))
            {
                MainWindow.Instance.Accept();
            }
            else
            {
                NotifyOfSelect(item);
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
                var x = _store.AppendValues(choice.GetChoiceText(), 1, choice);
                if (choice.IsSelected())
                {
                    Console.WriteLine(choice.GetChoiceText() + " is selected");
                    Selection.SelectIter(x);
                }
            }
            return this;
        }

        public JpgTreeView HandleSearchReturnKey()
        {
            var item = GetFirstRowItem();
            //_sortedModel.GetIterFirst(out var item);
            Console.WriteLine("return key give me " + GetValueFromIter(item)?.GetChoiceText());
            if (CheckForDoubleClickOrDoubleReturn(item))
            {
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
            _sortedModel.Model.GetIterFirst(out var iter);
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

        public JpgTreeView UpdateOrder(string searchText)
        {
            _store.GetIterFirst(out var iter);
            var temp = GetValueFromIter(iter);
            Console.WriteLine("the first item i'm updating is " + temp?.GetChoiceText());
            for (var i = 0; i < _store.IterNChildren(); i++)
            {
                var item = GetValueFromIter(iter);
                _store.SetValue(iter, (int) Column.SortValue, item.CalculateScore(searchText).GetScore());
                _store.IterNext(ref iter);
            }

            return this;
        }
    }
}