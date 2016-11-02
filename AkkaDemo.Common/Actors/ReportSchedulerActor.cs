using System;
using System.Collections.Generic;
using System.IO;
using Akka.Actor;
using AkkaDemo.Common.Messages;

namespace AkkaDemo.Common.Actors
{
    public class ReportSchedulerActor : BaseActor
    {
        public ReportSchedulerActor()
        {
            Receive<ReportMessage>(msg => HandleReportMessage(msg));
        }

        private void HandleReportMessage(ReportMessage msg)
        {
            var actor = Context.ActorOf(Props.Create(() => new ReportActor(msg.JobId)), $"Report_{msg.JobId}");
            actor.Forward(msg);
            actor.Tell(PoisonPill.Instance);
        }

        #region Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } PreStart on { Environment.MachineName }");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } PreRestart because: { reason }");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } PostRestart because: { reason }");
            base.PostRestart(reason);
        }
        #endregion
    }
}
