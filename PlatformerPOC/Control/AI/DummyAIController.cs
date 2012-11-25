using System;
using Microsoft.Xna.Framework;

namespace PlatformerPOC.Control.AI
{
    /// <summary>
    /// Stupid AI that runs around randomly and fires at will.
    /// </summary>
    public class DummyAIController : IPlayerControlState
    {
        private Vector2 ownPosition;
        private Vector2 playerPosition;

        // Test passing some 'environment' info to the ai mind

        public DummyAIController(Vector2 ownPosition, Vector2 playerPosition)
        {
            this.ownPosition = ownPosition;
            this.playerPosition = playerPosition;
        }

        public bool IsMoveLeftPressed
        {
            get
            {
                return ownPosition.X > playerPosition.X;
            }
        }

        public bool IsMoveRightPressed
        {
            get
            {
                return ownPosition.X < playerPosition.X;
            }
        }

        public bool IsMoveDownPressed
        {
            get
            {
                return ownPosition.Y < playerPosition.Y;
            }
        }

        public bool IsMoveUpPressed
        {
            get
            {
                return ownPosition.Y > playerPosition.Y;
            }
        }

        public bool IsActionPressed
        {
            get { return Math.Abs((playerPosition - ownPosition).Length()) < 200; }
        }
    }
}