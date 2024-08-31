using nthLink.Header.Enum;
using nthLink.Header.Interface;
using nthLink.Header.Struct;

namespace nthLink.SDK.Model
{
    public class EventBus<T> : IEventBus<T>
    {
        private readonly EventBusChannel<T> defaultChannel = new EventBusChannel<T>();
        private int subscriberCount = 0;
        private readonly Dictionary<string, EventBusChannel<T>> channels
            = new Dictionary<string, EventBusChannel<T>>();

        public int GetSubscriberCount()
        {
            return this.subscriberCount;
        }
        public int GetSubscriberCount(string channel)
        {
            if (this.channels.ContainsKey(channel))
            {
                return this.channels[channel].SubscriberCount;
            }
            else
            {
                return GetSubscriberCount();
            }
        }
        public void Subscribe(EventBusHandler<T> handler)
        {
            this.defaultChannel.Subscribe(handler);
            Interlocked.Increment(ref this.subscriberCount);
        }
        public void Subscribe(string channel, EventBusHandler<T> handler)
        {
            if (string.IsNullOrEmpty(channel))
            {
                Subscribe(handler);
            }
            else
            {
                if (!this.channels.ContainsKey(channel))
                {
                    this.channels.Add(channel, new EventBusChannel<T>());
                }

                this.channels[channel].Subscribe(handler);
            }
        }
        public void Unsubscribe(EventBusHandler<T> handler)
        {
            this.defaultChannel.Unsubscribe(handler);
            Interlocked.Decrement(ref this.subscriberCount);
        }
        public void Unsubscribe(string channel, EventBusHandler<T> handler)
        {
            if (string.IsNullOrEmpty(channel))
            {
                Unsubscribe(handler);
            }
            else
            {
                if (this.channels.ContainsKey(channel))
                {
                    this.channels[channel].Unsubscribe(handler);

                    //if (this.channels[channel].SubscriberCount == 0)
                    //{
                    //    this.channels.Remove(channel);
                    //}
                }
            }
        }
        public void Publish(string channel, T args)
        {
            if (string.IsNullOrEmpty(channel))
            {
                this.defaultChannel.Publish(channel, args);
            }
            else
            {
                if (this.channels.ContainsKey(channel))
                {
                    this.channels[channel].Publish(channel, args);
                }
            }
        }
    }

    class EventBusChannel<T>
    {
        private event EventBusHandler<T>? eventPublished;
        private int subscriberCount = 0;
        public int SubscriberCount => this.subscriberCount;
        public void Subscribe(EventBusHandler<T> handler)
        {
            this.eventPublished += handler;
            Interlocked.Increment(ref this.subscriberCount);
        }
        public void Unsubscribe(EventBusHandler<T> handler)
        {
            this.eventPublished -= handler;
            Interlocked.Decrement(ref this.subscriberCount);
        }
        public void Publish(string channel, T args)
        {
            if (this.eventPublished != null)
            {
                this.eventPublished.Invoke(channel, args);
            }
        }
    }
}
