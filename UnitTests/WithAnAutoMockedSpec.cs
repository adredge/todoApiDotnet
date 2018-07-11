using NUnit.Framework;
using AutoMoqCore;
using Moq;

namespace UnitTests
{
    public class WithAnAutoMockedSpec<T> where T : class
    {
        private AutoMoqer autoMocker;

        [SetUp]
        public void AutoMockSetup()
        {
            autoMocker = new AutoMoqer();
        }
        
        protected T classUnderTest => autoMocker.Create<T>();

        protected Mock<U> GetMock<U>() where U : class
        {
            return autoMocker.GetMock<U>();
        }

        protected TAny Any<TAny>()
        {
            return It.IsAny<TAny>();
        }
    }
}