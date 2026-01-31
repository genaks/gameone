using Core;

namespace Core.Services
{
    public interface IMessageBroker : IGameService
    {
        public delegate void EventDelegate<T>(T e) where T : IGameEvent;

        void Subscribe<T>(EventDelegate<T> del) where T : IGameEvent;
        void Unsubscribe<T>(EventDelegate<T> del) where T : IGameEvent;
        void Publish(IGameEvent gameEvent);
    }
}