using nthLink.Header.Enum;
using nthLink.Header.Struct;

namespace nthLink.Header.Interface
{
    public interface IEventBus<T>
    {
        void Subscribe(EventBusHandler<T> handler);
        void Unsubscribe(EventBusHandler<T> handler);
        void Subscribe(string channel, EventBusHandler<T> handler);
        void Unsubscribe(string channel, EventBusHandler<T> handler);
        void Publish(string channel, T args);
        int GetSubscriberCount();
        int GetSubscriberCount(string channel);
    }
}
