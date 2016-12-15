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
            ColorConsole.WriteLineYellow($"Calc V2 for {msg.LeftOperand} and {msg.RightOperand}");
            Thread.Sleep(5000);
            Sender.Tell(CalcVersionTwo(msg));
            ColorConsole.WriteLineCyan($"Result completed.");
        }

        private CalcResultMessage CalcVersionOne(CalcMessage msg)
        {
            var result = msg.LeftOperand + msg.RightOperand;
            return new CalcResultMessage(msg.JobId, 1, msg, result);
        }

        private CalcResultMessage CalcVersionTwo(CalcMessage msg)
        {
            var result = (msg.LeftOperand + msg.RightOperand) * 2;
            return new CalcResultMessage(msg.JobId, 2, msg, result);
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
