using System;
using System.Collections.Generic;
using System.IO;
using Akka.Actor;
using AkkaDemo.Common.Messages;

namespace AkkaDemo.Common.Actors
{
    public class ApiActor : BaseActor
    {
        private readonly IActorRef _logActor;
        private readonly IActorRef _reportActor;
        private readonly IActorRef _calcActor;

        public ApiActor(IActorRef logActor, IActorRef reportActor, IActorRef calcActor)
        {
            _logActor = logActor;
            _reportActor = reportActor;
            _calcActor = calcActor;

            Start();
        }

        private void Start()
        {
            Receive<LogEntryMessage>(msg => _logActor.Tell(msg));
            Receive<ReportMessage>(msg => _reportActor.Forward(msg));
            Receive<CalcMessage>(msg => _calcActor.Forward(msg));
        }

        #region Lifecycle hooks
        protected override void PreStart()
        {
//            ColorConsole.WriteLineYellow($"{ ActorClassName } PreStart.");
        }

        protected override void PostStop()
        {
//            ColorConsole.WriteLineYellow($"{ ActorClassName } { _appId } PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
//            ColorConsole.WriteLineYellow($"{ ActorClassName } PreRestart because: { reason }");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
//            ColorConsole.WriteLineYellow($"{ ActorClassName } PostRestart because: { reason }");
            base.PostRestart(reason);
        }
        #endregion
    }
}
