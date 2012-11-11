using GameEngine.GameObjects;
using NUnit.Framework;
using PlatformerPOC.GameObjects;

namespace PlatformerPOC.Tests
{
    [TestFixture]
    public class MainTest
    {
        [Test]
        public void t()
        {
            var p1 = new Player("Tester", 123, new GameObjectState());
            Assert.IsFalse(p1.IsAlive);
        }
    }
}