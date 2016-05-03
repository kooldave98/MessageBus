using MessageBus.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MessageBus.Tests
{
    public class PingHandler : IMessageHandler<Ping>
    {
        public void handle(Ping message)
        {
            handle_callback(message);
        }

        public PingHandler(Action<Ping> the_handle_callback)
        {
            handle_callback = the_handle_callback;
        }

        private Action<Ping> handle_callback;
    }

    public class PingPongHandler : IMessageHandler<Ping>, IMessageHandler<Pong>
    {
        public void handle(Ping message)
        {
            ping_handle_callback(message);
        }

        public void handle(Pong message)
        {
            pong_handle_callback(message);
        }

        public PingPongHandler(Action<Ping> the_ping_handle_callback
                             , Action<Pong> the_pong_handle_callback)
        {
            ping_handle_callback = the_ping_handle_callback;
            pong_handle_callback = the_pong_handle_callback;
        }

        private Action<Ping> ping_handle_callback;
        private Action<Pong> pong_handle_callback;
    }

    public sealed class MessageLogger
    {
        public IEnumerable<IMessage> messages { get { return the_messages; } }

        public IEnumerable<M> get_messages_of_type<M>() where M : IMessage, new()
        {
            return messages.Where(m => m.GetType() == typeof(M)).Select(m => (M)m);
        }

        public void add<M>(M message) where M : IMessage
        {
            the_messages.Add(message);
        }

        private ICollection<IMessage> the_messages = new List<IMessage>();
    }

    public class Ping : IMessage
    {
        public string message { get; } = "Ping";
    }

    public class Pong : IMessage
    {
        public string message { get; } = "Pong";
    }
}
