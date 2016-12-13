using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Akka.Actor;
using Akka.Routing;

namespace AkkaDemo.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected static ActorSystem WebActorSystem;

        public static IActorRef ApiActor { get; private set; }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            WebActorSystem = ActorSystem.Create("akkaDemo");
            ApiActor = WebActorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "api");
        }
    }
}
