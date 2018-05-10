using Gtk;

namespace ConsoleApp1
{
    internal class UserNotifier
    {
        public static void Error(string message)
        {
            var popup = new MessageDialog(MainWindow.Instance,
                DialogFlags.Modal | DialogFlags.DestroyWithParent,
                MessageType.Error,
                ButtonsType.Ok,
                message);
            popup.Run();
            popup.Destroy();
        }

        public static void Notify(string message)
        {
            MainWindow.Instance.UserNotify(message);
        }
    }
}