namespace MessageBus.Core
{
    public interface IMessageHandler<T> where T : IMessage
    {
        void handle(T message);
    }
}
