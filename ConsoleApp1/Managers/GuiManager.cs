using System;
using System.Collections.Generic;
using Gtk;

namespace ConsoleApp1
{
    internal class GuiManager
    {
        private MainWindow _gui;
        public Func<UserActionResult, bool> AcceptCallback { get; set; }

        public static GuiManager Instance { get; } = new GuiManager();

        private GuiManager()
        {
        }

        public void Go()
        {
            
            Application.Run();
        }

        public void GetChoice(IEnumerable<ITreeViewChoice> choices, string prompt)
        {
            GetChoice(false, choices, prompt);
        }

        public void GetChoices(IEnumerable<ITreeViewChoice> choices, string prompt)
        {
            GetChoice(true, choices, prompt);
        }

        private void GetChoice(bool multiSelect, IEnumerable<ITreeViewChoice> choices, string prompt,
            bool doReset = true)
        {
            _gui.Reset(doReset)
                .SetChoices(choices, prompt)
                .SetMultiSelect(multiSelect);
        }

        public CancellableObj<string> GetNonEmptySingleLineInput(string prompt, bool isPassword = false)
        {
            while (true)
            {
                var choice = GetSingleLineInput(prompt, isPassword);
                if (choice.ResponseType == UserActionResult.ResultType.Canceled
                    || choice.ResponseType == UserActionResult.ResultType.Accept && !choice.Result.Equals(""))
                    return choice;

                UserNotifier.Error("Error: Input cannot be an empty string");
            }
        }

        public void GetUserInput(JpgActionManager actionManager, string prompt)
        {
            switch (actionManager.CurrentActionProvider.InputType)
            {
                case InputType.Single:
                    GetChoice(actionManager.GetActions(), prompt);
                    break;
                case InputType.Multi:
                    GetChoices(actionManager.GetActions(), prompt);
                    break;
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

        public CancellableObj<string> GetSingleLineInput(string prompt, bool isPassword, string prepopulate = "")
        {
            var retVal = new CancellableObj<string> {ResponseType = UserActionResult.ResultType.Canceled};
            var popup = new MessageDialog(MainWindow.Instance,
                DialogFlags.Modal | DialogFlags.DestroyWithParent,
                MessageType.Question,
                ButtonsType.OkCancel,
                prompt) {DefaultResponse = ResponseType.Ok};
            var input = new Entry
            {
                Visibility = !isPassword,
                InvisibleChar = '*',
                ActivatesDefault = true,
                Text = prepopulate
            };
            popup.ContentArea.PackEnd(input, true, false, 5);
            popup.ShowAll();
            if (popup.Run() == (int) ResponseType.Ok)
            {
                retVal.ResponseType = UserActionResult.ResultType.Accept;
                retVal.Result = input.Text;
            }

            popup.Destroy();
            return retVal;
        }

        public CancellableObj<string> GetFolder(string prompt)
        {
            var retVal = new CancellableObj<string> {ResponseType = UserActionResult.ResultType.Canceled};
            var filechooser = new FileChooserDialog("Select ProjectFolder To Save ProgramProjectSetting Data",
                MainWindow.Instance, FileChooserAction.SelectFolder, "Cancel", ResponseType.Cancel,
                "Open", ResponseType.Accept);

            if (filechooser.Run() == (int) ResponseType.Accept)
            {
                retVal.ResponseType = UserActionResult.ResultType.Accept;
                retVal.Result = filechooser.Filename;
            }

            filechooser.Destroy();
            return retVal;
        }

        public CancellableObj<string> GetPassword()
        {
            return GetSingleLineInput("Enter Password", true);
        }

        public CancellableObj<string> GetSingleLineInput(string prompt, string currentValue)
        {
            return GetSingleLineInput(prompt, false, currentValue);
        }

        public CancellableObj<string> GetMultiLineInput(string prompt, string currentValue)
        {
            var retVal = new CancellableObj<string> {ResponseType = UserActionResult.ResultType.Canceled};
            var popup = new MessageDialog(MainWindow.Instance,
                DialogFlags.Modal | DialogFlags.DestroyWithParent,
                MessageType.Question,
                ButtonsType.OkCancel,
                prompt)
            {
                DefaultResponse = ResponseType.Ok,
                DefaultHeight = 500,
                DefaultWidth = 600
            };
            var input = new TextView {Buffer = {Text = currentValue}};
            popup.ContentArea.PackEnd(input, false, true, 15);
            popup.ShowAll();
            if (popup.Run() == (int) ResponseType.Ok)
            {
                retVal.ResponseType = UserActionResult.ResultType.Accept;
                retVal.Result = input.Buffer.Text;
            }

            popup.Destroy();
            return retVal;
        }

        public void GetReady(JpgActionManager jpgActionManager, string prompt)
        {
            Application.Init();
            MainWindow.Instance = new MainWindow();
            _gui = MainWindow.Instance;
            _gui.UserActionCallback = AcceptCallback;
            GetChoice(false, jpgActionManager.GetActions(), prompt, false);
        }

        public CancellableObj<string> GetFile(string prompt)
        {
             var retVal = new CancellableObj<string> {ResponseType = UserActionResult.ResultType.Canceled};
            var filechooser = new FileChooserDialog(prompt,
                MainWindow.Instance, FileChooserAction.Open, "Cancel", ResponseType.Cancel,
                "Open", ResponseType.Accept);

            if (filechooser.Run() == (int) ResponseType.Accept)
            {
                retVal.ResponseType = UserActionResult.ResultType.Accept;
                retVal.Result = filechooser.Filename;
            }

            filechooser.Destroy();
            return retVal;
        }
    }
}