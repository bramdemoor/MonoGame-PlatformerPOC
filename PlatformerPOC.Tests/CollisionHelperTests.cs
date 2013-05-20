using FluentAssertions;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Tests
{
    [TestFixture]
    public class CollisionHelperTests
    {
        [Test]
        public void RectangleCollision_False()
        {
            var col1 = CollisionHelper.RectangleCollision(new Rectangle(0, 0, 4, 4), new Rectangle(100, 100, 4, 4));
            col1.Should().BeFalse();
        }

        [Test]
        public void RectangleCollision_True()
        {
            var col1 = CollisionHelper.RectangleCollision(new Rectangle(0, 0, 4, 4), new Rectangle(0, 0, 2, 2));
            col1.Should().BeTrue();
        }
    }
}