using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Akka.Actor;
using AkkaDemo.Common.Messages;

namespace AkkaDemo.Common.Actors
{
    class ReportActor : BaseActor
    {
        private int _jobId; 

        public ReportActor(int jobId)
        {
            _jobId = jobId;
            Receive<ReportMessage>(rpt => HandleReportMessage(rpt));
        }

        private void HandleReportMessage(ReportMessage rpt)
        {
            Random rnd = new Random();
            var delay = rnd.Next(10, 20) * 1000;

            ColorConsole.WriteLineYellow($"Generating Report for Job #{rpt.JobId}. Should take {delay/1000}s to finish.it");
            Thread.Sleep(delay);
            ColorConsole.WriteLineCyan($"Report '{rpt.ReportTitle}' for Job #{rpt.JobId} completed.");
            Sender.Tell($"Report #{rpt.JobId} completed.");
        }

        #region Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } { _jobId } created.");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } { _jobId } PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } { _jobId } PreRestart because: { reason }");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } { _jobId } PostRestart because: { reason }");
            base.PostRestart(reason);
        }
        #endregion
    }
}
