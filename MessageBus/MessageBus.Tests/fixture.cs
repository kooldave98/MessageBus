using MessageBus.Core;
using System.Collections.Generic;

namespace MessageBus.Tests
{
    public class PingHandler : IMessageHandler<Ping>
    {
        public void handle(Ping message)
        {
            logger.add(message);
        }

        public MessageLogger logger = new MessageLogger();
    }

    public class PingPongHandler : IMessageHandler<Ping>, IMessageHandler<Pong>
    {
        public void handle(Ping message)
        {
            logger.add(message);
        }

        public void handle(Pong message)
        {
            logger.add(message);
        }

        public MessageLogger logger = new MessageLogger();
    }

    public sealed class MessageLogger
    {
        public IEnumerable<IMessage> messages { get { return the_messages; } }

        public void add<M>(M message)where M : IMessage
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
