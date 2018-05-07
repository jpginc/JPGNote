using System;
using System.Collections.Generic;
using System.Threading;
using Gtk;

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

        public UserActionResult GetChoice(IEnumerable<ITreeViewChoice> choices)
        {
            return GetChoice(false, choices);
        }

        public UserActionResult GetChoices(IEnumerable<ITreeViewChoice> choices)
        {
            return GetChoice(true, choices);
        }

        private UserActionResult GetChoice(bool multiSelect, IEnumerable<ITreeViewChoice> choices)
        {
            _gui.SetChoices(choices, _prompt)
                .SetMultiSelect(multiSelect);
            _waitForCallbackHandle.WaitOne();
            return _gui.GetUserActionResult();
        }
    }
}