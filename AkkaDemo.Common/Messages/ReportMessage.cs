using System;

namespace AkkaDemo.Common.Messages
{
    public class ReportMessage
    {
        public int JobId { get; set; }
        public string ReportTitle { get; set; }
    }
}
