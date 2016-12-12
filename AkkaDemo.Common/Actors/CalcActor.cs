using Akka.Actor;
using AkkaDemo.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AkkaDemo.Common.Actors
{
    public class CalcActor : BaseActor
    {
        public CalcActor()
        {
            Receive<CalcMessage>(msg => HandleCalcMessage(msg));
        }

        private void HandleCalcMessage(CalcMessage msg)
        {
            ColorConsole.WriteLineYellow($"Calc V1 for {msg.LeftOperand} and {msg.RightOperand}");
            Thread.Sleep(5000);
            var result = msg.LeftOperand + msg.RightOperand;
            Sender.Tell(new CalcResultMessage(msg, result));
            ColorConsole.WriteLineCyan($"Result completed.");
        }

        #region Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } created.");
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
