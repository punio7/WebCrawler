using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessReadWriteUtils;
using WebCrawler.Common.Enums;

namespace WebCrawler.WorkerApp.Common.Model
{
    public class ProcessInstance
    {
        public delegate void ProcessOutputEventHandler(ProcessInstance sender, string text);
        public event ProcessOutputEventHandler OutputRead = delegate { };
        public event Action<ProcessInstance> OnExit = delegate { };
        public int Pid { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public long SessionId { get; set; }
        public ProcessStatus Status { get; set; }
        public ObservableStringBuilder OutputBuffer { get; set; } = new ObservableStringBuilder();
        public Process Process { get; set; }
        public ProcessIoManager ProcessIoManager { get; set; }

        public ProcessInstance()
        {
            
        }

        public void DataReceivedEventHandler(object sender, DataReceivedEventArgs e)
        {
            HandleProcessOutput(e.Data);
        }

        public void StandardOutputEvent(string text)
        {
            HandleProcessOutput(text);
        }

        public void StandardErrorEvent(string text)
        {
            HandleProcessOutput(text);
        }

        public void ExitEvent(object sender, EventArgs e)
        {
            OnExit(this);
        }

        public void Kill()
        {
            if (Process != null)
            {
                Process.Kill();
                OnExit(this);
                Process = null;
            }
        }

        public void StartReadingOutput()
        {
            if (ProcessIoManager != null)
            {
                ProcessIoManager.StartProcessOutputRead();
            }
        }

        public void Dispose()
        {
            Kill();
        }

        private void HandleProcessOutput(string text)
        {
            OutputBuffer.Append(text);
            OutputRead(this, text);
        }
    }
}
