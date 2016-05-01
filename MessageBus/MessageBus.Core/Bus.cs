using System;
using System.Collections.Generic;
using System.Linq;

namespace MessageBus.Core
{
    public sealed class Bus
    {
        private Dictionary<Type, HashSet<object>> message_to_handlers = new Dictionary<Type, HashSet<object>>();
        public void SendMessage<T>(T message) where T : IMessage
        {

            var entry = message_to_handlers.SingleOrDefault(i => i.Key == typeof(T));

            if (entry.Key == null && entry.Value == null)
            {
                return;
            }

            foreach (dynamic item in entry.Value)
            {
                item.handle(message);
            }
        }

        public void Register(object Thandler)
        {
            var interfaces = get_handler_interfaces(Thandler);

            foreach (dynamic item in interfaces)
            {
                if (message_to_handlers.ContainsKey(item.message_type))
                {
                    message_to_handlers[item.message_type].Add(item.handler);
                }
                else
                {
                    message_to_handlers.Add(item.message_type, new HashSet<object>() { item.handler });
                }
            }
        }

        public void Deregister(object Thandler)
        {
            var interfaces = get_handler_interfaces(Thandler);

            foreach (dynamic item in interfaces)
            {
                if (!message_to_handlers.ContainsKey(item.message_type))
                {
                    return;
                }

                message_to_handlers[item.message_type].Remove(item.handler);
            }
        }

        private IEnumerable<object> get_handler_interfaces(object Thandler)
        {
            return
                Thandler
                    .GetType()
                    .GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMessageHandler<>))
                    .Select(i => new
                    {
                        handler_type = i,
                        handler = Thandler,
                        message_type = i.GetGenericArguments().Single()
                    });
        }

        public static Bus Instance => instance;
        private static readonly Bus instance = new Bus();
        static Bus() {/*Explicit static constructor to tell C# compiler not to mark type as beforefieldinit*/}
        private Bus(){/* private constructor is required for a singleton*/}
    }
}

