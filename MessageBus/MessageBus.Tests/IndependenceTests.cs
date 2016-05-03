using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageBus.Core;
using System.Linq;

namespace MessageBus.Tests
{
    [TestClass]
    public class IndependenceTests
    {
        [TestMethod]
        public void exception_in_one_handler_will_not_halt_execution_of_other_handlers()
        {
            //Arrange
            var ping_logger = new MessageLogger();
            var ping_handler = new PingHandler(m => { throw new System.Exception(); });

            var ping_logger2 = new MessageLogger();
            var ping_handler2 = new PingHandler(ping_logger2.add);

            Bus.Instance.Register(ping_handler);
            Bus.Instance.Register(ping_handler2);

            //Act
            Bus.Instance.SendMessage(new Ping());

            //Assert
            Assert.AreEqual(1, ping_logger2.messages.Count());
        }

        [TestMethod]
        public void async_execution_of_handlers_will_not_block_message_sender()
        {
            //Arrange
            var ping_logger = new MessageLogger();
            var ping_handler = new PingHandler(ping_logger.add);

            Bus.Instance.Register(ping_handler);

            //Act
            Bus.Instance.SendMessageAsync(new Ping());

            //Assert
            Assert.AreEqual(0, ping_logger.messages.Count());
        }
    }
}
