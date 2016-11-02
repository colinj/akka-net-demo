using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Akka.Actor;
using AkkaDemo.Common.Messages;

namespace AkkaDemo.Common.Actors
{
    class ReportActor : ReceiveActor
    {
        private const string ActorTypeDesc = "ReportActor";
        private int _jobId { get; set; }

        public ReportActor(int jobId)
        {
            _jobId = jobId;
            Receive<ReportMessage>(rpt => HandleReportMessage(rpt));
        }

        private void HandleReportMessage(ReportMessage rpt)
        {
            Random rnd = new Random();
            var delay = rnd.Next(10, 20) * 1000;

            ColorConsole.WriteLineYellow($"Generating Report for Job #{rpt.JobId}.");
            Thread.Sleep(delay);
            ColorConsole.WriteLineCyan($"Report '{rpt.ReportTitle}' for Job #{rpt.JobId} completed.");
            Sender.Tell($"Report #{rpt.JobId} completed.");
        }

        #region Lifecycle hooks
        protected override void PreStart()
        {
//            ColorConsole.WriteLineYellow($"Report actor { _jobId } created.");
        }

        protected override void PostStop()
        {
//            ColorConsole.WriteLineYellow($"{ ActorTypeDesc } { _jobId } PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineYellow($"{ ActorTypeDesc } { _jobId } PreRestart because: { reason }");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineYellow($"{ ActorTypeDesc } { _jobId } PostRestart because: { reason }");
            base.PostRestart(reason);
        }
        #endregion
    }
}
