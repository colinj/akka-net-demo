using System;

namespace AkkaDemo.Common.Messages
{
    public class ReportMessage
    {
        public int JobId { get; }
        public string ReportTitle { get; }

        public ReportMessage(int id, string title)
        {
            JobId = id;
            ReportTitle = title;
        }
    }

}
