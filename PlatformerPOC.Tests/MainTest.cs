using GameEngine.GameObjects;
using Microsoft.Xna.Framework;
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
            var game = new PlatformGame();

            var p1 = new Player(game, "Tester", 123, new GameObjectState());
            Assert.IsFalse(p1.IsAlive);


            p1.Spawn(new Vector2(0,0));

            Assert.IsTrue(p1.IsAlive);
        }
    }
}