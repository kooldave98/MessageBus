using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageBus.Core;
using System.Linq;

namespace MessageBus.Tests
{
    [TestClass]
    public class NHandlerTests
    {        

        [TestMethod]
        public void will_publish_one_of_each_message_to_double_handler()
        {
            //Arrange
            var ping_pong_handler = new PingPongHandler();

            Bus.Instance.Register(ping_pong_handler);

            //Act
            Bus.Instance.SendMessage(new Ping());
            Bus.Instance.SendMessage(new Pong());

            //Assert
            Assert.AreEqual(2, ping_pong_handler.logger.messages.Count());
            Assert.AreEqual(1, ping_pong_handler.logger.get_messages_of_type<Ping>().Count());
            Assert.AreEqual(1, ping_pong_handler.logger.get_messages_of_type<Pong>().Count());
        }

        [TestMethod]
        public void will_publish_manytimes_to_double_handler()
        {
            //Arrange
            var ping_pong_handler = new PingPongHandler();

            Bus.Instance.Register(ping_pong_handler);

            //Act
            Enumerable
                .Range(0, 5)
                .ToList()
                .ForEach(i =>
                {
                    Bus.Instance.SendMessage(new Ping());
                    Bus.Instance.SendMessage(new Pong());
                });
            

            //Assert
            Assert.AreEqual(5, ping_pong_handler.logger.get_messages_of_type<Ping>().Count());
            Assert.AreEqual(5, ping_pong_handler.logger.get_messages_of_type<Pong>().Count());
        }

        [TestMethod]
        public void will_publish_to_many_double_handlers()
        {
            //Arrange
            var ping_pong_handler = new PingPongHandler();
            var ping_pong_handler2 = new PingPongHandler();

            Bus.Instance.Register(ping_pong_handler);
            Bus.Instance.Register(ping_pong_handler2);

            //Act            
            Bus.Instance.SendMessage(new Ping());
            Bus.Instance.SendMessage(new Pong());

            //Assert
            Assert.AreEqual(1, ping_pong_handler.logger.get_messages_of_type<Ping>().Count());
            Assert.AreEqual(1, ping_pong_handler.logger.get_messages_of_type<Pong>().Count());
            Assert.AreEqual(1, ping_pong_handler2.logger.get_messages_of_type<Ping>().Count());
            Assert.AreEqual(1, ping_pong_handler2.logger.get_messages_of_type<Pong>().Count());
        }


        [TestMethod]
        public void will_publish_only_to_registered_double_handler()
        {
            //Arrange
            var ping_pong_handler = new PingPongHandler();
            var ping_pong_handler2 = new PingPongHandler();

            Bus.Instance.Register(ping_pong_handler);

            //Act            
            Bus.Instance.SendMessage(new Ping());
            Bus.Instance.SendMessage(new Pong());

            //Assert
            Assert.AreEqual(1, ping_pong_handler.logger.get_messages_of_type<Ping>().Count());
            Assert.AreEqual(1, ping_pong_handler.logger.get_messages_of_type<Pong>().Count());
            Assert.AreEqual(0, ping_pong_handler2.logger.get_messages_of_type<Ping>().Count());
            Assert.AreEqual(0, ping_pong_handler2.logger.get_messages_of_type<Pong>().Count());
        }

        [TestMethod]
        public void will_not_publish_to_deregistered_double_handler()
        {
            //Arrange
            var ping_pong_handler = new PingPongHandler();

            Bus.Instance.Register(ping_pong_handler);

            //Act            
            Bus.Instance.SendMessage(new Ping());
            Bus.Instance.SendMessage(new Pong());
            Bus.Instance.Deregister(ping_pong_handler);
            Bus.Instance.SendMessage(new Ping());
            Bus.Instance.SendMessage(new Pong());

            //Assert
            Assert.AreEqual(1, ping_pong_handler.logger.get_messages_of_type<Ping>().Count());
            Assert.AreEqual(1, ping_pong_handler.logger.get_messages_of_type<Pong>().Count());
        }
    }
}
