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
            //toRun:= ComSpec " /c " this.fixWindowsCommandString(this.sshLocation,
            //["-i", getSetting("vmOutputLocation") "\ssh\" vm "\id_rsa",
            //"user@" ip, " | ", this.teeLocation, logLocation])

            RunExe(_cmdLocation, "");
        }

        private void RunExe(string exeFileName, string args)
        {
            //todo this probably isn't cross platform...
            var compiler = new Process();
            compiler.StartInfo.FileName = exeFileName;
            compiler.StartInfo.Arguments = args;
            compiler.StartInfo.UseShellExecute = true;
            compiler.StartInfo.RedirectStandardOutput = false;
            compiler.Start();
        }
    }
}