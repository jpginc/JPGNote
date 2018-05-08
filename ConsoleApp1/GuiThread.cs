using System.Threading;
using Gtk;
using Action = System.Action;

namespace ConsoleApp1
{
    internal class GuiThread
    {
        public static void Go(Action action)
        {
            var ev = new ManualResetEvent(false);
            Application.Invoke((a, b) => {
                action();
                ev.Set();
            });
            ev.WaitOne();
        }

        public static void DontWait(Action action)
        {
            Application.Invoke((a, b) => action());
        }
    }
}