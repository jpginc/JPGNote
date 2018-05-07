using System;
using System.Collections.Generic;
using System.Threading;
using GLib;
using Application = Gtk.Application;
using System.Threading;
using Thread = System.Threading.Thread;


namespace ConsoleApp1
{
    internal class GuiManager
    {
        private MainWindow _gui;
        private readonly string _prompt = "It's time to choose...";
        private readonly AutoResetEvent _waitForCallbackHandle = new AutoResetEvent(false);

        public GuiManager(string prompt) : this()
        {
            _prompt = prompt;
        }
        public GuiManager()
        {
            new Thread(() =>
                {
                    Application.Init();
                    _gui = new MainWindow("JPG Tree", AcceptCallback);
                    Application.Run();
                }
            ).Start();

            Thread.Sleep(1000);

        }

        private bool AcceptCallback()
        {
            return _waitForCallbackHandle.Set();
        }

        public ITreeViewChoice GetChoice(IEnumerable<ITreeViewChoice> choices)
        {
            GetChoice(false, choices);
            Console.WriteLine("Got a single choice!");
            return null;
        }

        public IEnumerable<ITreeViewChoice> GetChoices(IEnumerable<ITreeViewChoice> choices)
        {
            GetChoice(true, choices);
            Console.WriteLine("Got lots of choices");
            return null;
        }

        private IEnumerable<ITreeViewChoice> GetChoice(bool multiSelect, IEnumerable<ITreeViewChoice> choices)
        {
            //new Thread(() => { 
                if (choices == null)
                {
                    throw new Exception("shit");
                }
                _gui.SetChoices(choices, _prompt)
                .SetMultiSelect(multiSelect);
                Console.WriteLine("Hello, world");
            //}).Start();
            _waitForCallbackHandle.WaitOne();
            //todo
            return null;
        }
    }
}