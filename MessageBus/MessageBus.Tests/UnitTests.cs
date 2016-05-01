using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageBus.Core;
using System.Linq;

namespace MessageBus.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void will_publish_once_to_single_handler()
        {
            //Arrange
            var ping_handler = new PingHandler();

            Bus.Instance.Register(ping_handler);

            //Act
            Bus.Instance.SendMessage(new Ping());

            //Assert
            Assert.AreEqual(1, ping_handler.logger.messages.Count());
        }

        [TestMethod]
        public void will_publish_manytimes_to_single_handler()
        {
            //Arrange
            var ping_handler = new PingHandler();

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
            Assert.AreEqual(5, ping_handler.logger.messages.Count());
        }

        [TestMethod]
        public void will_publish_only_to_registered_single_handler()
        {
            //Arrange
            var ping_handler = new PingHandler();
            var ping_handler2 = new PingHandler();

            Bus.Instance.Register(ping_handler);

            //Act            
            Bus.Instance.SendMessage(new Ping());

            //Assert
            Assert.AreEqual(1, ping_handler.logger.messages.Count());
            Assert.AreEqual(0, ping_handler2.logger.messages.Count());
        }

        [TestMethod]
        public void will_not_publish_to_deregistered_single_handler()
        {
            //Arrange
            var ping_handler = new PingHandler();

            Bus.Instance.Register(ping_handler);

            //Act            
            Bus.Instance.SendMessage(new Ping());
            Bus.Instance.Deregister(ping_handler);
            Bus.Instance.SendMessage(new Ping());

            //Assert
            Assert.AreEqual(1, ping_handler.logger.messages.Count());
        }
    }
}
