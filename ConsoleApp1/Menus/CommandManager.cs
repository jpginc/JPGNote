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
        private static string _autohotkeyLocation = "C:\\Program Files\\AutoHotkey\\AutoHotkey.exe";
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

        public void RunCommand(string commandString, Project project, UserAction userAction)
        {
            var logLocation = project.GetLogFileFullLocation();

            var args = MachineManager.Instance.GetSshCommandLineArgs() + $" \"{commandString}\"";
            RunExeToFile(_sshLocation, args, logLocation, userAction);
        }

        private void RunExeToFile(string exeFileName, string args, string logLocation, UserAction userAction)
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
                File.WriteAllText(logLocation, "Command not complete yet.\nit may never finish");
                //var file = File.Open(logLocation, FileMode.Create, FileAccess.Write, FileShare.Read);
                var output = "";
                while (!p.HasExited)
                {
                    int nextChar; 
                    if (p.StandardOutput.Peek() > -1)
                    {
                        nextChar = p.StandardOutput.Read();
                        //file.WriteByte((byte) nextChar);
                        output += (char) nextChar;
                    }
                    if (p.StandardError.Peek() > -1)
                    {
                        nextChar = p.StandardError.Read();
                        //file.WriteByte((byte) nextChar);
                        output += (char) nextChar;
                    }

                    Thread.Sleep(0);
                }

                //var remaining = p.StandardOutput.ReadToEnd();
                output += p.StandardOutput.ReadToEnd();
                //file.Write(Encoding.UTF8.GetBytes(remaining), 0, remaining.Length);
                //remaining = p.StandardError.ReadToEnd();
                output += p.StandardError.ReadToEnd();
                File.WriteAllText(logLocation, output);
                //file.Write(Encoding.UTF8.GetBytes(remaining), 0, remaining.Length);
                //file.Close();
                p.Close();
                ParseOutput(logLocation, userAction);
            }).Start();
        }

        private void ParseOutput(string outputLocation, UserAction userAction)
        {
            if (File.Exists(userAction.ParsingCodeLocation))
            {
                var p = new Process
                {
                    StartInfo =
                    {
                        FileName = _autohotkeyLocation,
                        Arguments = "\"" + userAction.ParsingCodeLocation + "\""
                            + " \"" + outputLocation + "\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true
                    }
                };
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                ParseOutput(output);
                p.Close();
            }
        }

        private void ParseOutput(string output)
        {
            using (StringReader sr = new StringReader(output))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Equals("Target"))
                    {
                        var target = sr.ReadLine();
                        if (target != null)
                        {
                            lock (TargetManager.Instance.Creatables)
                            {
                                TargetManager.Instance.Creatables.Add(new Target() {IpOrDomain = target});
                            }
                            TargetManager.Instance.Save();
                        }
                    }
                }
            }
        }
    }
}