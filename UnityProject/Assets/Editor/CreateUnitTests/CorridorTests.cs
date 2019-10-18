using Moq;
using NUnit.Framework;

namespace Editor.UnitTestExampleClasses
{
    public class CorridorTests
    {
        private Corridor _model;
        private Mock<ISomeDependency> _someDependencyMock;

        [SetUp]
        public void Setup()
        {
            _someDependencyMock = new Mock<ISomeDependency>();
            _model = new Corridor(_someDependencyMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            //todo
        }

        [Test]
        public void Test_GetMoney()
        {
            SetMoney(10);
        }

        private void SetMoney(int money)
        {
            _someDependencyMock.Setup(someDep => someDep.GetMoney()).Returns(money);
        }
    }
}