using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaDemo.Common.Messages
{
    public class CalcMessage
    {
        public int JobId { get; }
        public int LeftOperand { get; }
        public int RightOperand { get; }

        public CalcMessage(int jobId, int left, int right)
        {
            JobId = jobId;
            LeftOperand = left;
            RightOperand = right;
        }
    }

    public class CalcResultMessage
    {
        public int JobId { get; }
        public int Version { get; }
        public int LeftOperand { get; }
        public int RightOperand { get; }
        public int Result { get; }

        public CalcResultMessage(int jobId, int version, CalcMessage msg, int result)
        {
            JobId = jobId;
            Version = version;
            LeftOperand = msg.LeftOperand;
            RightOperand = msg.RightOperand;
            Result = result;
        }
    }
}
