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
            var ev = new ManualResetEvent(false);
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
            //Thread.Sleep(100);
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

        private UserActionResult GetChoice(bool multiSelect, IEnumerable<ITreeViewChoice> choices, string prompt,
            bool doReset = true)
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


        public CancellableObj<string> GetNonEmptySingleLineInput(string prompt, bool resetGui = true)
        {
            while (true)
            {
                var choice = GetSingleLineInput(prompt);
                if (choice.ResponseType == UserActionResult.ResultType.Canceled
                    || choice.ResponseType == UserActionResult.ResultType.Accept && !choice.Result.Equals(""))
                    return choice;

                UserNotifier.Error("Error: Input cannot be an empty string");
            }
        }

        public UserActionResult GetUserInput(JpgActionManager actionManager, string prompt)
        {
            switch (actionManager.CurrentActionProvider.InputType)
            {
                case InputType.Single:
                    return GetChoice(actionManager.GetActions(), prompt);
                case InputType.Multi:
                    return GetChoices(actionManager.GetActions(), prompt);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private UserActionResult GetSingleLineInputFromGui(string prompt, bool resetGui = true)
        {
            var choice = new[] {(ITreeViewChoice) new SelectingTriggersAcceptAction("Press enter to finish input")};
            GetChoice(false, choice, prompt, resetGui);
            return _gui.GetUserActionResult();
        }

        public UserActionResult GetNonEmptySingleLineInputFromGui(string prompt, bool resetGui = true)
        {
            while (true)
            {
                var choice = GetSingleLineInputFromGui(prompt, resetGui);
                if (choice.Result == UserActionResult.ResultType.Canceled
                     || choice.Result == UserActionResult.ResultType.Accept && !choice.SingleLineInput.Equals(""))
                    return choice;

                UserNotifier.Error("Error: Input cannot be an empty string");
                resetGui = false;
            }
        }

        public CancellableObj<string> GetSingleLineInput(string prompt)
        {
            return GetSingleLineInput(prompt, false);
        }

        public CancellableObj<string> GetSingleLineInput(string prompt, bool isPassword)
        {
            var retVal = new CancellableObj<string> {ResponseType = UserActionResult.ResultType.Canceled};
            GuiThread.Go(() =>
            {
                lock (retVal)
                {
                    var popup = new MessageDialog(MainWindow.Instance,
                        DialogFlags.Modal | DialogFlags.DestroyWithParent,
                        MessageType.Question,
                        ButtonsType.OkCancel,
                        prompt) {DefaultResponse = ResponseType.Ok};
                    var input = new Entry
                    {
                        Visibility = !isPassword,
                        InvisibleChar = '*',
                        ActivatesDefault = true
                    };
                    popup.ContentArea.PackEnd(input, true, false, 5);
                    popup.ShowAll();
                    if (popup.Run() == (int) ResponseType.Ok)
                    {
                        retVal.ResponseType = UserActionResult.ResultType.Accept;
                        retVal.Result = input.Text;
                    }

                    popup.Destroy();
                }
            });
            return retVal;
        }

        public CancellableObj<string> GetFolder(string prompt)
        {
            var retVal = new CancellableObj<string> {ResponseType = UserActionResult.ResultType.Canceled};
            GuiThread.Go(() =>
            {
                lock (retVal)
                {
                    var filechooser = new FileChooserDialog("Select Folder To Save ProgramProjectSetting Data",
                        MainWindow.Instance, FileChooserAction.SelectFolder, "Cancel", ResponseType.Cancel,
                        "Open", ResponseType.Accept);

                    if (filechooser.Run() == (int) ResponseType.Accept)
                    {
                        retVal.ResponseType = UserActionResult.ResultType.Accept;
                        retVal.Result = filechooser.Filename;
                    }

                    filechooser.Destroy();
                }
            });
            return retVal;
        }

        public CancellableObj<string> GetPassword()
        {
            return GetSingleLineInput("Enter Password", true);
        }
    }
}