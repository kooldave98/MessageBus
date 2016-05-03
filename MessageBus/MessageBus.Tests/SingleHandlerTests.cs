using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageBus.Core;
using System.Linq;

namespace MessageBus.Tests
{
    [TestClass]
    public class SingleHandlerTests
    {
        [TestMethod]
        public void will_publish_once_to_single_handler()
        {
            //Arrange
            var ping_logger = new MessageLogger();
            var ping_handler = new PingHandler(ping_logger.add);

            Bus.Instance.Register(ping_handler);

            //Act
            Bus.Instance.SendMessage(new Ping());

            //Assert
            Assert.AreEqual(1, ping_logger.messages.Count());
        }

        [TestMethod]
        public void will_publish_manytimes_to_single_handler()
        {
            //Arrange
            var ping_logger = new MessageLogger();
            var ping_handler = new PingHandler(ping_logger.add);

            Bus.Instance.Register(ping_handler);

            //Act
            Enumerable
                .Range(0, 5)
                .ToList()
                .ForEach(i =>
                {
                    Bus.Instance.SendMessage(new Ping());
                });
            

            //Assert
            Assert.AreEqual(5, ping_logger.messages.Count());
        }

        [TestMethod]
        public void will_publish_to_many_single_handlers()
        {
            //Arrange
            var ping_logger = new MessageLogger();
            var ping_handler = new PingHandler(ping_logger.add);

            var ping_logger2 = new MessageLogger();
            var ping_handler2 = new PingHandler(ping_logger2.add);

            Bus.Instance.Register(ping_handler);
            Bus.Instance.Register(ping_handler2);

            //Act            
            Bus.Instance.SendMessage(new Ping());

            //Assert
            Assert.AreEqual(1, ping_logger.messages.Count());
            Assert.AreEqual(1, ping_logger2.messages.Count());
        }


        [TestMethod]
        public void will_publish_only_to_registered_single_handler()
        {
            //Arrange
            var ping_logger = new MessageLogger();
            var ping_handler = new PingHandler(ping_logger.add);

            var ping_logger2 = new MessageLogger();
            var ping_handler2 = new PingHandler(ping_logger2.add);

            Bus.Instance.Register(ping_handler);

            //Act            
            Bus.Instance.SendMessage(new Ping());

            //Assert
            Assert.AreEqual(1, ping_logger.messages.Count());
            Assert.AreEqual(0, ping_logger2.messages.Count());
        }

        [TestMethod]
        public void will_not_publish_to_deregistered_single_handler()
        {
            //Arrange
            var ping_logger = new MessageLogger();
            var ping_handler = new PingHandler(ping_logger.add);

            Bus.Instance.Register(ping_handler);

            //Act            
            Bus.Instance.SendMessage(new Ping());
            Bus.Instance.Deregister(ping_handler);
            Bus.Instance.SendMessage(new Ping());

            //Assert
            Assert.AreEqual(1, ping_logger.messages.Count());
        }
    }
}
