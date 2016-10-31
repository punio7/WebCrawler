﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.HubModel.Arguments;
using WebCrawler.WorkerApp.Common.Model;
using WebCrawler.WorkerApp.Logic.Factories;

namespace WebCrawler.WorkerApp.Logic.Managers
{
    public class ProcessManager
    {
        public delegate void ProcessAddedEventHandler(ProcessInstance newProcess);
        public event ProcessAddedEventHandler ProcessAdded = delegate { };
        public Dictionary<long, ProcessInstance> ProcessInstances { get; set; } = new Dictionary<long, ProcessInstance>();

        private ProcessInstanceFactory ProcessInstanceFactory { get; set; }

        public ProcessManager(ProcessInstanceFactory processInstanceFactory)
        {
            ProcessInstanceFactory = processInstanceFactory;
        }

        public ProcessInstance CreateNewProcessForLocalhost(string executablePath)
        {
            FileInfo executableFile = new FileInfo(executablePath);
            var newProcess = ProcessInstanceFactory.CreateAndStartProcess(executableFile);
            newProcess.Name = executableFile.Name;
            newProcess.User = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            newProcess.SessionId = new Random().Next();
            newProcess.StartReadingOutput();
            ProcessInstances[newProcess.SessionId] = newProcess;
            ProcessAdded(newProcess);
            return newProcess;
        }

        public ProcessInstance CreateNewProcess(StartProcessArguments args)
        {
            FileInfo executableFile = new FileInfo(@"D:\workspace\Dungeon-Crawler\Debug\dungeon_crawler.exe");
            var newProcess = ProcessInstanceFactory.CreateAndStartProcess(executableFile);
            newProcess.Name = args.ApplicationName;
            newProcess.User = args.UserName;
            newProcess.SessionId = args.SessionId;
            ProcessInstances[newProcess.SessionId] = newProcess;
            ProcessAdded(newProcess);
            return newProcess;
        }

        public void StopProcess(long sessionId)
        {
            var processInstance = GetProcessInstance(sessionId);
            processInstance.Kill();
            ProcessInstances.Remove(sessionId);
        }

        public void ExecuteCommand(long sessionId, string command)
        {
            var processInstance = GetProcessInstance(sessionId);
            processInstance.OutputBuffer.AppendLine(command);
            command += Environment.NewLine;
            var bytes = Encoding.Unicode.GetBytes(command);
            processInstance.Process.StandardInput.BaseStream.Write(bytes, 0, bytes.Length);
            processInstance.Process.StandardInput.Flush();
        }

        private ProcessInstance GetProcessInstance(long sessionId)
        {
            if (ProcessInstances.ContainsKey(sessionId))
            {
                return ProcessInstances[sessionId];
            }
            else
            {
                throw new InvalidOperationException($"Brak procesu o Session Id: {sessionId}");
            }

        }
    }
}
