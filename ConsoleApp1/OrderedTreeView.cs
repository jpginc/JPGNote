using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;

namespace ConsoleApp1
{
    class OrderedTreeView 
    {
        private ListStore Store = new ListStore(typeof(string), typeof(ITreeViewChoice));
        public TreeView GuiElement { get; }

        public OrderedTreeView()
        {
            GuiElement = new TreeView(Store) {HeadersVisible = false};
            var valueColumn = new TreeViewColumn();
            GuiElement.AppendColumn(valueColumn);
            var visisbleColumnTextRenderer = new CellRendererText();
            valueColumn.PackStart(visisbleColumnTextRenderer , true);
            valueColumn.AddAttribute(visisbleColumnTextRenderer, "text", 0);
        }

        public OrderedTreeView SetChoices(IEnumerable<ITreeViewChoice> choices)
        {
            Store.Clear();
            foreach (var choice in choices.OrderBy(c => c))
            {
                TreeIter x = Store.AppendValues(choice.GetChoiceText(), choice);
                if (choice.IsSelected())
                {
                    GuiElement.Selection.SelectIter(x);
                }
            }
            return this;
        }

        

    }
}
