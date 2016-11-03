using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Akka.Actor;
using AkkaDemo.Common.Messages;

namespace AkkaDemo.Common.Actors
{
    public class ReportActor : BaseActor
    {
        private static Random Rnd = new Random();
        private static int NextId = 0;
        private int _actorId;

        public ReportActor()
        {
            _actorId = ++NextId;
            Receive<ReportMessage>(rpt => HandleReportMessage(rpt));
        }

        private void HandleReportMessage(ReportMessage rpt)
        {
            var delay = Rnd.Next(10, 20);

            ColorConsole.WriteLineYellow($"Generating Report for Job #{rpt.JobId}. Should take { delay }s to finish.it");
            Thread.Sleep(delay * 1000);
            ColorConsole.WriteLineCyan($"{ ActorClassName } { _actorId }: Report '{rpt.ReportTitle}' for Job #{rpt.JobId} completed.");
            Sender.Tell($"Report #{rpt.JobId} completed.");
        }

        #region Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } { _actorId } created.");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } { _actorId } PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } { _actorId } PreRestart because: { reason }");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } { _actorId } PostRestart because: { reason }");
            base.PostRestart(reason);
        }
        #endregion
    }
}
