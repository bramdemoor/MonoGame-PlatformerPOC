using System;
using Microsoft.Xna.Framework;

namespace GameEngine.GameObjects
{
    public class GameObjectState : ICloneable
    {        
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        private float rotation;
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                if (rotation == value % MathHelper.TwoPi)
                {
                    return;
                }

                rotation = value % MathHelper.TwoPi;
            }
        }

        

        public object Clone()
        {
            return new GameObjectState { Position = Position, Rotation = Rotation, Velocity = Velocity };
        }
    }
}