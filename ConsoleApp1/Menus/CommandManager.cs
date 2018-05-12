using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

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
            RunRedirectedShell(_cmdLocation, args);
        }

        private string GetOuputRedirectionString(string logLocation)
        {
            return $"\"{_teeLocation}\" \"{logLocation}\"";
        }

        private void RunRedirectedShell(string exeFileName, string args)
        {
            //todo this isn't cross platform...
            var p = new Process
            {
                StartInfo =
                {
                    FileName = exeFileName,
                    Arguments = args,
                    UseShellExecute = true,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                }
            };
            p.Start();
        }

        public void RunCommand(string commandString, Project project)
        {
            var logLocation = project.GetLogFileFullLocation();

            var args = MachineManager.Instance.GetSshCommandLineArgs() + $" \"{commandString}\"";
            RunExeToFile(_sshLocation, args, logLocation);
        }

        private void RunExeToFile(string exeFileName, string args, string logLocation)
        {
            new Thread(() =>
            {

                var p = new Process
                {
                    StartInfo =
                    {
                        FileName = exeFileName,
                        Arguments = args,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };
                p.Start();
                var file = File.Open(logLocation, FileMode.Create, FileAccess.Write, FileShare.Read);
                while (!p.HasExited)
                {
                    if (p.StandardOutput.Peek() > -1)
                    {
                        var nextChar = p.StandardOutput.Read();
                        file.WriteByte((byte) nextChar);
                    }

                    if (p.StandardError.Peek() > -1)
                    {
                        var nextChar = p.StandardError.Read();
                        file.WriteByte((byte) nextChar);
                    }

                    Thread.Sleep(0);
                }

                var remaining = p.StandardOutput.ReadToEnd();
                file.Write(Encoding.UTF8.GetBytes(remaining), 0, remaining.Length);
                remaining = p.StandardError.ReadToEnd();
                file.Write(Encoding.UTF8.GetBytes(remaining), 0, remaining.Length);
                file.Close();
                p.Close();
            }).Start();
        }
    }
}