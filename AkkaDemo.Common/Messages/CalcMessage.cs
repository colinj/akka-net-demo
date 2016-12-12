using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaDemo.Common.Messages
{
    public class CalcMessage
    {
        public int LeftOperand { get; }
        public int RightOperand { get; }

        public CalcMessage(int left, int right)
        {
            LeftOperand = left;
            RightOperand = right;
        }
    }

    public class CalcResultMessage
    {
        public int LeftOperand { get; }
        public int RightOperand { get; }
        public int Result { get; }

        public CalcResultMessage(CalcMessage msg, int result)
        {
            LeftOperand = msg.LeftOperand;
            RightOperand = msg.RightOperand;
            Result = result;
        }
    }
}
