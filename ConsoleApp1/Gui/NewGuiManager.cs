using System;
using System.Linq;
using System.Collections.Generic;
using Gtk;


namespace ConsoleApp1
{
    public static class NewGuiManager
    {
        private static List<MainGui> guis = new List<MainGui>();

        public static MainGui NewWindow()
        {
            var gui = new MainGui();
            guis.Add(gui);
            return gui;
        }

        public static void GuiClosed(object sender, DeleteEventArgs e)
        {
            Console.WriteLine("here");
            var w = sender as MainWindow;
            if(w == null) {
                Console.WriteLine("I don't think we should be getting here");
            }
            if(guis.Count == 1) {
                if(UserNotifier.Confirm("Are you sure you want to exit?", w)) 
                {
                    Application.Quit(); 
                    //Environment.Exit(0); // is this the right one? 
                } else {
                    e.RetVal = true; //block the window closing
                }
            } else 
            {
                var gui = guis.RemoveAll(g => g.HasWindow(w));
            }
        }

    }
    public class MainGui
    {
        private MainWindow _gui;

        public MainGui() {
            _gui = new MainWindow();
        }

        public bool HasWindow(MainWindow w) 
        {
            return _gui == w;
        }

        public void Notify(string message) 
        {
            _gui.UserNotify(message);
        }
        public CancellableObj<string> GetNonEmptySingleLineInput(string prompt, bool isPassword = false)
        {
            while (true)
            {
                var choice = GetSingleLineInput(prompt, isPassword);
                if (choice.ResponseType == UserActionResult.ResultType.Canceled
                    || choice.ResponseType == UserActionResult.ResultType.Accept && !choice.Result.Equals(""))
                    return choice;

                UserNotifier.Error("Error: Input cannot be an empty string", _gui);
            }
        }
        public CancellableObj<string> GetSingleLineInput(string prompt, bool isPassword, string prepopulate = "")
        {
            var retVal = new CancellableObj<string> {ResponseType = UserActionResult.ResultType.Canceled};
            var popup = new MessageDialog(_gui,
                DialogFlags.Modal | DialogFlags.DestroyWithParent,
                MessageType.Question,
                ButtonsType.OkCancel,
                prompt)
            {
                DefaultResponse = ResponseType.Ok,
                DefaultWidth = 600
            };
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


    }

}
