using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using Gtk;
using Action = System.Action;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    internal class CommandManager
    {
        [DataMember] private List<JobDetails> _queue = new List<JobDetails>();

        [IgnoreDataMember] public static CommandManager Instance { get; } = new CommandManager();
        [IgnoreDataMember] private static readonly string _sshLocation = "C:\\Program Files\\Git\\usr\\bin\\ssh.exe";
        [IgnoreDataMember] private static readonly string _teeLocation = "C:\\Program Files\\Git\\usr\\bin\\tee.exe";
        [IgnoreDataMember] private static readonly string _autohotkeyLocation = "C:\\Program Files\\AutoHotkey\\AutoHotkey.exe";
        [IgnoreDataMember] private static readonly string _cmdLocation = "C:\\Windows\\System32\\cmd.exe";
        [IgnoreDataMember] private int _running;


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
            var jobDetails = new JobDetails(commandString, target, port, userAction, project);

            _queue.Add(jobDetails);
            project.CommandQueued(jobDetails);
            QueueMove();
        }

        public void ReviveCommand(JobDetails job)
        {
            _queue.Add(job);
            QueueMove();
        }

        private void QueueDone(JobDetails job)
        {
            job.Project.CommandDone(job);
            _running--;
            QueueMove();
        }

        private void QueueMove()
        {
            Console.WriteLine($"Progressing queue. {_running} running. Total{_queue.Count}");
            if (_running < 15)
            {
                var toRun = _queue.FirstOrDefault();
                if (toRun != null)
                {
                    _running++;
                    _queue = _queue.Skip(1).ToList();
                    var args = MachineManager.Instance.GetSshCommandLineArgs() + $" \"{toRun.CommandString}\"";
                    var logLocation = toRun.Project.GetLogFileFullLocation(toRun.UserAction, toRun.Target);
                    toRun.LogLocation = logLocation;
                    RunExeToFile(_sshLocation, args, toRun);
                }
            }
        }

        private void RunExeToFile(string exeFileName, string args, JobDetails job)
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
                var file = File.Open(job.LogLocation, FileMode.Create, FileAccess.Write, FileShare.Read);
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
                if (job.Port != null) job.Port.Notes += "\n" + output;
                ParseOutput(job);
            }).Start();
        }

        private void ParseOutput(JobDetails job)
        {
            if (File.Exists(job.UserAction.ParsingCodeLocation))
            {
                var p = new Process
                {
                    StartInfo =
                    {
                        FileName = _autohotkeyLocation,
                        Arguments = "\"" + job.UserAction.ParsingCodeLocation + "\""
                                    + " \"" + job.LogLocation + "\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true
                    }
                };
                p.Start();
                var output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                ParseOutput(output, job);
                p.Close();
            }

            Application.Invoke((a, b) => QueueDone(job));
        }

        private void ParseOutput(string output, JobDetails job)
        {
            Application.Invoke((a, b) =>
            {
                using (var sr = new StringReader(output))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        if (line.Equals("Target"))
                        {
                            var discoveredTarget = sr.ReadLine();
                            if (discoveredTarget != null)
                            {
                                job.Project.TargetManager.AddPremade(new Target {IpOrDomain = discoveredTarget});
                                job.Project.TargetManager.Save();
                            }
                        }
                        else if (line.Equals("Port"))
                        {
                            var portNumber = sr.ReadLine();
                            var port = new Port
                            {
                                PortNumber = portNumber,
                                Target = job.Target
                            };
                            var notes = "";
                            while (!(line = sr.ReadLine()).Equals("Done")) notes += line + " ";

                            port.Notes = notes;
                            Console.WriteLine("Adding port " + port);
                            job.Project.PortManager.AddPremade(port);
                            job.Project.PortManager.Save();
                        }
                }
            });
        }
    }

    public class JobDetails
    {
        public readonly UserAction UserAction;
        public string CommandString { get; }
        public string LogLocation { get; set; }
        public string Target { get; }
        public Port Port { get; }
        public Project Project { get; }

        public JobDetails(string commandString, string target, Port port,
            UserAction userAction, Project project)
        {
            UserAction = userAction;
            CommandString = commandString;
            Target = target;
            Port = port;
            Project = project;
        }
    }
}