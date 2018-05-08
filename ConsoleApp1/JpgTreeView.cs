using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;

namespace ConsoleApp1
{
    internal class JpgTreeView : TreeView
    {
        private readonly ListStore _store = new ListStore(typeof(string), typeof(ITreeViewChoice));

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
            ((ITreeViewChoice) _store.GetValue(item, (int) Column.Value))?
                .SetSelected(Selection.IterIsSelected(item));
            return this;
        }

        private void ClickHandler(object o, RowActivatedArgs args)
        {
            Console.WriteLine("Clicked");
            _store.GetIter(out var item, args.Path);
            SetSelected(item, true);
            SearchEntry.GrabFocusWithoutSelecting();
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
            //GuiThread.Go(() =>
            //{
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
            //});
            return this;
        }

        public JpgTreeView ToggleTopItem()
        {
            //GuiThread.Go(() =>
            //{
                _store.GetIterFirst(out var item);
                ToggleSelect(item);
            //});
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
            //GuiThread.Go(() =>
            //{
                retVal = Selection.GetSelectedRows().Select(p =>
                {
                    _store.GetIter(out var item, p);
                    return (ITreeViewChoice) _store.GetValue(item, (int) Column.Value);
                }).ToList();
            //});
            return retVal;
        }
    }
}