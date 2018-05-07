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
        }

        public JpgTreeView SetChoices(IEnumerable<ITreeViewChoice> choices)
        {
            _store.Clear();
            foreach (var choice in choices)
            {
                TreeIter x = _store.AppendValues(choice.GetChoiceText(), choice);
                if (choice.IsSelected())
                {
                    Selection.SelectIter(x);
                }
            }
            return this;
        }

        public JpgTreeView ToggleTopItem()
        {
            TreeIter item;
            _store.GetIterFirst(out item);
            if (Selection.IterIsSelected(item))
            {
                Selection.UnselectIter(item);
            }
            else
            {
                Selection.SelectIter(item);
            }

            return this;
        }
    }
}
