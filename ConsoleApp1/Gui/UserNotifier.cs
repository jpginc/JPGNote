using Gtk;

namespace ConsoleApp1
{
    internal class UserNotifier
    {
        public static void Error(string message, MainWindow window)
        {
            var popup = new MessageDialog(window,
                    DialogFlags.Modal | DialogFlags.DestroyWithParent,
                    MessageType.Error,
                    ButtonsType.Ok,
                    message)
                {Expand = false};
            popup.Run();
            popup.Destroy();
        }

        public static void Notify(string message, MainWindow window)
        {
            window.UserNotify(message);
        }

        public static bool Confirm(string message, MainWindow window)
        {
            var popup = new MessageDialog(window,
                    DialogFlags.Modal | DialogFlags.DestroyWithParent,
                    MessageType.Warning,
                    ButtonsType.YesNo,
                    message)
                {Expand = false};
            var resp = popup.Run();
            popup.Destroy();
            return resp == (int) ResponseType.Yes;
        }
    }
}