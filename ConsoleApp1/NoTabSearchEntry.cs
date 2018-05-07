using System;
using Gdk;
using Gtk;
using Key = Gdk.Key;

namespace ConsoleApp1
{
    internal class NoTabSearchEntry : SearchEntry
    {
        protected override bool OnKeyPressEvent(EventKey evnt)
        {
            if (evnt.Key == Key.Tab)
            {
                Console.WriteLine("tab pressed");
                return true;
            }
            return base.OnKeyPressEvent(evnt);
        }
    }
}