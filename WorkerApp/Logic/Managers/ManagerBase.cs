using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace WebCrawler.WorkerApp.Logic.Managers
{
    public class ManagerBase
    {
        private ILog logger;

        public ManagerBase()
        {
            logger = LogManager.GetLogger(GetType());
        }

        public void Operation(Action action, params object[] args)
        {
            string methodName = new StackTrace().GetFrame(1).GetMethod().Name;
            string className = this.GetType().Name;
            string parameters = String.Join(", ", args);

            logger.Info($"Operation {className}.{methodName}({parameters})");
            try
            {
                action();
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw;
            }
        }
    }
}
