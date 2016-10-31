using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using ProcessReadWriteUtils;
using WebCrawler.Common.Enums;
using WebCrawler.WorkerApp.Common.Model;

namespace WebCrawler.WorkerApp.Logic.Factories
{
    public class ProcessInstanceFactory
    {
        public ProcessInstance CreateAndStartProcess(FileInfo executableFile)
        {
            Random random = new Random();
            var processInstance = new ProcessInstance()
            {
                Status = ProcessStatus.Running,
            };

            ProcessStartInfo info = new ProcessStartInfo(executableFile.FullName);
            info.UseShellExecute = false;
            //info.Arguments = args;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.CreateNoWindow = true;
            info.StandardOutputEncoding = Encoding.Unicode;

            var process = processInstance.Process = Process.Start(info);
            processInstance.Pid = process.Id;

            processInstance.ProcessIoManager = new ProcessIoManager(process);
            processInstance.ProcessIoManager.StderrTextRead += processInstance.StandardErrorEvent;
            processInstance.ProcessIoManager.StdoutTextRead += processInstance.StandardOutputEvent;

            //process.OutputDataReceived += processInstance.DataReceivedEventHandler;
            //process.BeginOutputReadLine();

            return processInstance;
        }
    }
}
