using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Akka.Actor;
using AkkaDemo.Common.Messages;
using AkkaDemo.WebAPI.Models;

namespace AkkaDemo.WebAPI.Controllers
{
    public class CalcController : ApiController
    {
        public async Task<CalcResultMessage> Post(CalcRequest request)
        {
            return await WebApiApplication.ApiActor.Ask<CalcResultMessage>(new CalcMessage(request.LeftOperand, request.RightOperand)); 
        }
    }
}
