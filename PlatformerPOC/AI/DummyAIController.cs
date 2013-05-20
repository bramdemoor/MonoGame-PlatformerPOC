using System;
using Microsoft.Xna.Framework;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.AI
{
    /// <summary>
    /// Stupid AI that runs around randomly and fires at will.
    /// </summary>
    public class DummyAIController : IPlayerControlState
    {
        public bool IsMoveLeftPressed { get; private set; }
        public bool IsMoveRightPressed { get; private set; }
        public bool IsMoveDownPressed { get; private set; }
        public bool IsMoveUpPressed { get; private set; }
        public bool IsActionPressed { get; private set; }

        private int skipLimit = 9;
        private int skipCounter;

        // Test passing some 'environment' info to the ai mind

        public void Evaluate(Vector2 ownPosition, Vector2 playerPosition, Random random)
        {
            if (skipCounter < skipLimit)
            {
                skipCounter++;
                return;
            }

            IsMoveLeftPressed = ownPosition.X > playerPosition.X;
            IsMoveRightPressed = ownPosition.X < playerPosition.X;
            IsMoveDownPressed = ownPosition.Y < playerPosition.Y;
            IsMoveUpPressed = ownPosition.Y > playerPosition.Y;
            IsActionPressed = Math.Abs((playerPosition - ownPosition).Length()) < 200;

            skipCounter = 0;
            skipLimit = random.Next(99);
        }
    }
}