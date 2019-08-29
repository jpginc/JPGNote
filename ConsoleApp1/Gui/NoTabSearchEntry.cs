using System;
using ConsoleApp1.BuiltInActions;
using Gdk;
using Gtk;
using Key = Gdk.Key;

namespace ConsoleApp1
{
    internal class NoTabSearchEntry : SearchEntry
    {
        private SearchableTreeView _parent;

        public NoTabSearchEntry(SearchableTreeView parent) : base()
        {
            _parent = parent;
            KeyPressEvent += MyHandler;
        }

        //this is the only way to capture the up and down keys. doesn't get passed to 
        //the onkeypressevent handler
        private void MyHandler(object o, KeyPressEventArgs args)
        {
            var evnt = args.Event;
            if (evnt.Key == Key.Down || evnt.Key == Key.Up)
            {
            }
            
        }

        //this is the only way to supress tab's change focus sideaffect
        [GLib.ConnectBefore]
        protected override bool OnKeyPressEvent(EventKey evnt)
        {
            if (evnt.Key == Key.Next || evnt.Key == Key.Prior)
            {
                _parent.HandlePageDownUp();
            }
            if (evnt.State == ModifierType.ControlMask)
            {
                if (evnt.Key == Key.o)
                {
                    //CommandManager.Instance.OpenSshSession(ProgramSettingsClass.Instance.Project);
                    return true;
                }
                if(evnt.Key == Key.a)
                {
                    _parent.SelectAll();
                    return true;
                }
                return base.OnKeyPressEvent(evnt);
            }
            if (evnt.Key == Key.Tab || evnt.Key == Key.ISO_Left_Tab)
            {
                _parent.HandleRotateKeypress(evnt);
                return true;
            }
            if (evnt.Key == Gdk.Key.d && evnt.State == Gdk.ModifierType.Mod1Mask)
            {
                Console.WriteLine("this event");
                _parent.HandleDone();
            }

            return base.OnKeyPressEvent(evnt);
        }
    }
}