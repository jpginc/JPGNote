using System.Diagnostics;

namespace ConsoleApp1.BuiltInActions
{
    internal class CommandManager
    {
        public static CommandManager Instance { get; } = new CommandManager();

        private static string _sshLocation = "C:\\Program Files\\Git\\usr\\bin\\ssh.exe";
        private static string _teeLocation = "C:\\Program Files\\Git\\usr\\bin\\tee.exe";
        private static readonly string _cmdLocation = "C:\\Windows\\System32\\cmd.exe";


        public void OpenSshSession(Project project)
        {
            var logLocation = project.GetLogFileFullLocation();

            var args = " /c \"" + MachineManager.Instance.GetSshCommandLineString() + " | "
                + GetOuputRedirectionString(logLocation) + "\"";
            RunExe(_cmdLocation, args);
        }

        private string GetOuputRedirectionString(string logLocation)
        {
            return $"\"{_teeLocation}\" \"{logLocation}\"";
        }

        private void RunExe(string exeFileName, string args)
        {
            //todo this isn't cross platform...
            var compiler = new Process();
            compiler.StartInfo.FileName = exeFileName;
            compiler.StartInfo.Arguments = args;
            compiler.StartInfo.UseShellExecute = true;
            compiler.StartInfo.RedirectStandardOutput = false;
            compiler.StartInfo.RedirectStandardError = false;
            compiler.Start();
        }
    }
}