using System;
using System.Collections.Generic;
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
            ManualResetEvent ev = new ManualResetEvent(false);
            //this shit isn't thread safe
            new Thread(() =>
                {
                    Application.Init();
                    _gui = MainWindow.Instance;
                    _gui.UserActionCallback = AcceptCallback;
                    ev.Set();
                    Application.Run();
                }
            ).Start();
            ev.WaitOne();
        }

        private bool AcceptCallback()
        {
            //todo fix threads
            //Thread.Sleep(300);
            _waitForCallbackHandle.Set();
            return true;
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
            return GetChoice(multiSelect, choices, prompt, true);
        }

        private UserActionResult GetChoice(bool multiSelect, IEnumerable<ITreeViewChoice> choices, string prompt,
            bool doReset)
        {
            _gui.Reset(doReset)
                .SetChoices(choices, prompt)
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
            return GetSingleLineInput(prompt, true);
        }

        public UserActionResult GetNonEmptySingleLineInput(string prompt)
        {
            var resetGui = true;
            while (true)
            {
                var choice = GetSingleLineInput(prompt, resetGui);
                if (choice.Result == UserActionResult.ResultType.Canceled 
                    || (choice.Result == UserActionResult.ResultType.Accept && !choice.SingleLineInput.Equals("")))
                {
                    return choice;
                }

                UserNotifier.Error("Error: Input cannot be an empty string");
                resetGui = false;
            }
        }

        private UserActionResult GetSingleLineInput(string prompt, bool resetGui)
        {
            var choice = new ITreeViewChoice[] {new AcceptOnSelectTreeViewChoice("Press enter to finish input")};
            GetChoice(false, choice, prompt, resetGui);
            return _gui.GetUserActionResult();
        }
    }
}