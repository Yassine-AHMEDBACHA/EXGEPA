using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CORESI.Tools
{
    public class ScoopLogger : IDisposable
    {
        public string WorkContext { get; private set; }

        public ILog Logger { get; private set; }

        public TimeSpan LastSnap { get; set; }

        Stopwatch StopWatch { get; set; }

        bool logAllOperationDuration { get; set; }

        public ScoopLogger(string workContext, ILog logger, bool logAllOperationDuration = true)
        {
            this.Logger = logger;
            this.WorkContext = workContext;
            this.logAllOperationDuration = logAllOperationDuration;
            StopWatch = Stopwatch.StartNew();
        }

        public void Snap(string task)
        {
            this.StopWatch.Stop();
            Logger.Info(task + " done in : " + (this.StopWatch.Elapsed - LastSnap));
            LastSnap = this.StopWatch.Elapsed;
            this.StopWatch.Start();
        }

        public void Dispose()
        {
            this.StopWatch.Stop();
            if (logAllOperationDuration)
                Logger.Info(WorkContext + " done in : " + this.StopWatch.Elapsed);
        }
    }
}
