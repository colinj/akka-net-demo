using Akka.Actor;

namespace AkkaDemo.Common.Actors
{
    public abstract class BaseActor : ReceiveActor
    {
        protected string ActorClassName => GetType().Name;
    }
}
