using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkkaDemo.WebAPI.Models
{
    public class CalcRequest
    {
        public int JobId { get; set; }
        public int LeftOperand { get; set; }
        public int RightOperand { get; set; }
    }
}