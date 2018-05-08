using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;

namespace ConsoleApp1
{
    internal class JpgTreeView : TreeView
    {
        private readonly ListStore _store = new ListStore(typeof(string), typeof(ITreeViewChoice));
        private DateTime _lastClick = DateTime.Now;
        private string _lastText;

        private enum Column
        {
            Text,
            Value
        }

        public JpgTreeView SetMultiSelect(bool doMulti)
        {
            //GuiThread.Go(() =>
            //{
                Selection.Mode = doMulti ? SelectionMode.Multiple : SelectionMode.Single;
            //});
            return this;
        }

        public JpgTreeView(SearchEntry search)
        {
            Model = _store;
            HeadersVisible = false;
            SearchEntry = search;
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
            GetValueFromIter(item)?.SetSelected(Selection.IterIsSelected(item));
            return this;
        }

        private ITreeViewChoice GetValueFromIter(TreeIter item)
        {
            return (ITreeViewChoice) _store.GetValue(item, (int) Column.Value);
        }

        private bool CheckForDoubleClickOrDoubleReturn(TreeIter item)
        {
            var cellText = GetValueFromIter(item)?.GetChoiceText();
            var retVal = (DateTime.Now - _lastClick).Seconds < 0.5 && Equals(_lastText, cellText);
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
                var x = _store.AppendValues(choice.GetChoiceText(), choice);
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
            _store.GetIterFirst(out var item);
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
    }
}