using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gdk;
using Gtk;

namespace ConsoleApp1
{
    internal class JpgTreeView : TreeView
    {
        private readonly ListStore _store = new ListStore(typeof(string), typeof(ITreeViewChoice));

        enum Column
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
            SearchEntry = search;
            var valueColumn = new TreeViewColumn();
            AppendColumn(valueColumn);
            var visisbleColumnTextRenderer = new CellRendererText();
            valueColumn.PackStart(visisbleColumnTextRenderer , true);
            valueColumn.AddAttribute(visisbleColumnTextRenderer, "text", 0);
            ActivateOnSingleClick = true;
            RowActivated += ClickHandler;
        }

        private JpgTreeView UpdateSelectedItem(TreeIter item)
        {
            ((ITreeViewChoice) _store.GetValue(item, (int) Column.Value))
                    .SetSelected(Selection.IterIsSelected(item));
            return this;
        }

        private void ClickHandler(object o, RowActivatedArgs args)
        {
            Console.WriteLine("Clicked");
            _store.GetIter(out var item, args.Path);
            UpdateSelectedItem(item);
            SearchEntry.GrabFocusWithoutSelecting();
        }

        public JpgTreeView SetChoices(IEnumerable<ITreeViewChoice> choices)
        {
            _store.Clear();
            foreach (var choice in choices)
            {
                TreeIter x = _store.AppendValues(choice.GetChoiceText(), choice);
                if (choice.IsSelected())
                {
                    Console.WriteLine("It is selected");
                    Selection.SelectIter(x);
                }
            }
            return this;
        }

        public JpgTreeView ToggleTopItem()
        {
            _store.GetIterFirst(out var item);
            if (Selection.IterIsSelected(item))
            {
                Selection.UnselectIter(item);
            }
            else
            {
                Selection.SelectIter(item);
            }
            return UpdateSelectedItem(item);
        }
    }
}
