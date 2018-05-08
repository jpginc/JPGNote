using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gtk;

namespace ConsoleApp1
{
    internal class GuiManager
    {
        private MainWindow _gui;
        public static GuiManager Instance { get; } = new GuiManager();
        private readonly AutoResetEvent _waitForCallbackHandle = new AutoResetEvent(false);

        private GuiManager()
        {
            //this shit isn't thread safe
            new Thread(() =>
                {
                    Application.Init();
                    _gui = MainWindow.Instance;
                    _gui.UserActionCallback = AcceptCallback;
                    Application.Run();
                }
            ).Start();
            Thread.Sleep(1000); 
        }

        private bool AcceptCallback()
        {
            return _waitForCallbackHandle.Set();
        }

        public UserActionResult GetChoice(IEnumerable<ITreeViewChoice> choices, string prompt)
        {
            return GetChoice(false, choices, prompt);
        }

        public UserActionResult GetChoices(IEnumerable<ITreeViewChoice> choices, string prompt)
        {
            return GetChoice(true, choices, prompt);
        }

        private UserActionResult GetChoice(bool multiSelect, IEnumerable<ITreeViewChoice> choices, string prompt)
        {
            _gui.SetChoices(choices, prompt)
                .SetMultiSelect(multiSelect);
            WaitForCallback();
            return _gui.GetUserActionResult();
        }

        private void WaitForCallback()
        {
            _waitForCallbackHandle.WaitOne();
            _waitForCallbackHandle.Reset();
        }

        public UserActionResult GetSingleLineInput(string prompt)
        {
            _gui.SetChoices(Enumerable.Empty<ITreeViewChoice>(), prompt)
                .SetMultiSelect(false);
            _waitForCallbackHandle.WaitOne();
            return _gui.GetUserActionResult();
        }
    }
}