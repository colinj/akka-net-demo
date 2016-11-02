using System;
using System.Collections.Generic;
using System.IO;
using Akka.Actor;
using AkkaDemo.Common.Messages;

namespace AkkaDemo.Common.Actors
{
    public class ReportSchedulerActor : ReceiveActor
    {
        private const string ActorTypeDesc = "ReportSchedulerActor";

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
            ColorConsole.WriteLineYellow($"{ ActorTypeDesc } PreStart on { Environment.MachineName }");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineYellow($"{ ActorTypeDesc } PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineYellow($"{ ActorTypeDesc } PreRestart because: { reason }");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineYellow($"{ ActorTypeDesc } PostRestart because: { reason }");
            base.PostRestart(reason);
        }
        #endregion
    }
}
