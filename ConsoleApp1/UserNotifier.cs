using System;
using System.Threading;
using Gtk;

namespace ConsoleApp1
{
    internal class UserNotifier
    {
        public static void Error(string message)
        {
            //new Thread(() => { MainWindow.Instance.Error(message, AcceptCallback); }).Start();
            MainWindow.Instance.Error(message);
        }

        public static void Notify(string message)
        {
            MainWindow.Instance.UserNotify(message);
        }
    }
}