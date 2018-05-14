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

        public void RunCommand(string commandString, Project project, 
            UserAction userAction, string target, Port port)
        {
            var logLocation = project.GetLogFileFullLocation(userAction, target);

            var args = MachineManager.Instance.GetSshCommandLineArgs() + $" \"{commandString}\"";
            RunExeToFile(_sshLocation, args, logLocation, userAction, target, port);
        }

        private void RunExeToFile(string exeFileName, string args, string logLocation, UserAction userAction,
            string target, Port port)
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
                //File.WriteAllText(logLocation, "Command not complete yet.\nit may never finish");
                var file = File.Open(logLocation, FileMode.Create, FileAccess.Write, FileShare.Read);
                var output = "";
                while (!p.HasExited)
                {
                    int nextChar; 
                    if (p.StandardOutput.Peek() > -1)
                    {
                        nextChar = p.StandardOutput.Read();
                        file.WriteByte((byte) nextChar);
                        output += (char) nextChar;
                    }
                    if (p.StandardError.Peek() > -1)
                    {
                        nextChar = p.StandardError.Read();
                        file.WriteByte((byte) nextChar);
                        output += (char) nextChar;
                    }

                    Thread.Sleep(0);
                }

                //output += p.StandardOutput.ReadToEnd();
                //output += p.StandardError.ReadToEnd();
                //File.WriteAllText(logLocation, output);
                var remaining = p.StandardOutput.ReadToEnd();
                output += remaining;
                file.Write(Encoding.UTF8.GetBytes(remaining), 0, remaining.Length);
                remaining = p.StandardError.ReadToEnd();
                output += remaining;
                file.Write(Encoding.UTF8.GetBytes(remaining), 0, remaining.Length);
                file.Close();
                p.Close();
                if (port != null)
                {
                    port.Notes += "\n" + output;
                }
                ParseOutput(logLocation, userAction, target);
            }).Start();
        }

        private void ParseOutput(string outputLocation, UserAction userAction, string target)
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
                ParseOutput(output, target);
                p.Close();
            }
        }

        private void ParseOutput(string output, string target)
        {
            using (StringReader sr = new StringReader(output))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Equals("Target"))
                    {
                        var discoveredTarget = sr.ReadLine();
                        if (discoveredTarget != null)
                        {
                            TargetManager.Instance.AddPremade(new Target() {IpOrDomain = discoveredTarget});
                            TargetManager.Instance.Save();
                        }
                    } else if (line.Equals("Port"))
                    {
                        var portNumber = sr.ReadLine();
                        var port = new Port
                        {
                            PortNumber = portNumber,
                            Target = target
                        };
                        var notes = "";
                        while(! (line = sr.ReadLine()).Equals("Done"))
                        {
                            notes += line + " ";
                        }
                        port.Notes = notes;
                        Console.WriteLine("Adding port " + port);
                        PortManager.Instance.AddPremade(port);
                        PortManager.Instance.Save();
                    }
                }
            }
        }
    }
}