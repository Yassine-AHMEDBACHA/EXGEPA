// <copyright file="ScoopLogger.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools
{
    using System;
    using System.Diagnostics;
    using log4net;

    public class ScoopLogger : IDisposable
    {
        private readonly Stopwatch stopWatch;

        public ScoopLogger(string workContext, ILog logger, bool logAllOperationDuration = true)
        {
            this.Logger = logger;
            this.WorkContext = workContext;
            this.LogAllOperationDuration = logAllOperationDuration;
            this.stopWatch = Stopwatch.StartNew();
        }

        public string WorkContext { get; private set; }

        public ILog Logger { get; private set; }

        public TimeSpan LastSnap { get; set; }

        public bool LogAllOperationDuration { get; private set; }

        public void Snap(string task)
        {
            this.stopWatch.Stop();
            this.Logger.Info(task + " done in : " + (this.stopWatch.Elapsed - this.LastSnap));
            this.LastSnap = this.stopWatch.Elapsed;
            this.stopWatch.Start();
        }

        public void Dispose()
        {
            this.stopWatch.Stop();
            if (this.LogAllOperationDuration)
            {
                this.Logger.Info(this.WorkContext + " done in : " + this.stopWatch.Elapsed);
            }
        }
    }
}
