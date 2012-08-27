﻿using GameEngine;
using GameEngine.GameObjects;
using GameEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.GameObjects
{
    public class Player : PlatformGameObject
    {
        private const int MAX_LIFE = 100;
        private const int MOVE_SPEED = 5;

        public string Name { get; set; }

        public int Life { get; set; }

        public bool IsAlive
        {
            get { return Life > 0; }
        }

        public bool IsStandingOnSolid
        {
            get
            {
                return !PlatformGame.Instance.Level.IsPlaceFree(BoundingBox_Bottom);
            }
        }

        public Rectangle BoundingBox_Full
        {
            get
            {
                return new Rectangle((int) Position.X - 10 , (int) Position.Y + 6, spriteSheetInstance.SpriteSheetDefinition.SpriteDimensions.Width + 20, spriteSheetInstance.SpriteSheetDefinition.SpriteDimensions.Height - 6);
            }
        }

        public Rectangle BoundingBox_Top
        {
            get
            {
                // TODO BDM: Put velocity add elsewhere?
                return new Rectangle(BoundingBox_Full.X, (int) (BoundingBox_Full.Top + Velocity.Y), BoundingBox_Full.Width, 1);
            }
        }

        public Rectangle BoundingBox_Bottom
        {
            get
            {
                // TODO BDM: Put velocity add elsewhere?
                return new Rectangle(BoundingBox_Full.X, (int) (BoundingBox_Full.Bottom + Velocity.Y), BoundingBox_Full.Width, 1);
            }
        }

        public Rectangle BoundingBox_Left
        {
            get
            {
                return new Rectangle((int) (BoundingBox_Full.X + Velocity.X), BoundingBox_Full.Y, 1, BoundingBox_Full.Height);
            }
        }

        public Rectangle BoundingBox_Right
        {
            get
            {
                return new Rectangle((int) (BoundingBox_Full.Right + Velocity.X), BoundingBox_Full.Y, 1, BoundingBox_Full.Height);
            }
        }

        //public Vector2 DesiredNextPosition
        //{
        //    get { return Position + Velocity; }
        //}

        //public Rectangle DesiredNextPositionRectangle
        //{
        //    get { return new Rectangle((int)DesiredNextPosition.X, (int)DesiredNextPosition.Y, spriteSheetInstance.SpriteSheetDefinition.SpriteDimensions.Width, spriteSheetInstance.SpriteSheetDefinition.SpriteDimensions.Height); }
        //}

        private readonly CustomSpriteSheetInstance spriteSheetInstance;
        
        private PlayerKeyboardState playerKeyboardState;

        public Rectangle RectangleCollisionBounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, spriteSheetInstance.SpriteSheetDefinition.SpriteDimensions.Width, spriteSheetInstance.SpriteSheetDefinition.SpriteDimensions.Height);
            }
        }

        private bool IsMovingHorizontally
        {
            get { return playerKeyboardState.IsMoveLeftPressed || playerKeyboardState.IsMoveRightPressed; }
        }

        public Player(string name, long id, GameObjectState gameObjectState)
        {
            spriteSheetInstance = new CustomSpriteSheetInstance(ResourcesHelper.PlayerSpriteSheet);

            this.Name = name;

            Spawn();            
        }

        public void DoDamage(int damage)
        {
            if(IsAlive)
            {
                Life -= damage;
            }
        }

        public void Spawn()
        {
            Life = MAX_LIFE;

            PlaySound(ResourcesHelper.SpawnSound);            

            Position = PlatformGame.Instance.Level.GetNextFreeSpawnPoint();
        }

        public void Update()
        {
            if(IsAlive)
            {
                ApplyInput();
            }

            var rect = new Rectangle((int)(Position.X), (int)Position.Y, spriteSheetInstance.SpriteSheetDefinition.SpriteDimensions.Width, spriteSheetInstance.SpriteSheetDefinition.SpriteDimensions.Height);
            if (!PlatformGame.Instance.Level.IsPlaceFree(rect))
            {
                Velocity = new Vector2(Velocity.X, 0);
            }

            VerticalMovement();
            HorizontalMovement();
            

            if (IsMovingHorizontally)
            {
                spriteSheetInstance.LoopWithReverse();
            }            
        }

        private void HorizontalMovement()
        {
            if(Velocity.X > 0)
            {
                if(PlatformGame.Instance.Level.IsPlaceFree(BoundingBox_Right))
                {
                    Position = new Vector2(Position.X + Velocity.X, Position.Y);
                }
            }

            if (Velocity.X < 0)
            {
                if (PlatformGame.Instance.Level.IsPlaceFree(BoundingBox_Left))
                {
                    Position = new Vector2(Position.X + Velocity.X, Position.Y);
                }
            }        
        }

        private void VerticalMovement()
        {
            // TOP CHECK
            if(Velocity.Y < 0)
            {
                if (!PlatformGame.Instance.Level.IsPlaceFree(BoundingBox_Top))
                {
                    Velocity = new Vector2(Velocity.X, 0);
                }
            }

            if (!IsStandingOnSolid)
            {
                Velocity += new Vector2(0, 0.2f);
            }
            else
            {
                if (Velocity.Y > 0)
                {
                    Velocity = new Vector2(Velocity.X, 0);
                }
            }

             Position = new Vector2(Position.X, Position.Y + Velocity.Y);
        }

        private void ApplyInput()
        {
            if (playerKeyboardState.IsMoveLeftPressed)
            {
                Velocity = new Vector2(- MOVE_SPEED, Velocity.Y);
                HorizontalDirection = -1;
            }
            else
            {
                if (playerKeyboardState.IsMoveRightPressed)
                {
                    Velocity = new Vector2(MOVE_SPEED, Velocity.Y);
                    HorizontalDirection = 1;
                }
                else
                {
                    Velocity = new Vector2(0, Velocity.Y);
                }
            }

            if (playerKeyboardState.IsMoveUpPressed)
            {
                Jump();
            }

            if (playerKeyboardState.IsActionPressed)
            {
                Shoot();
            }            
        }

        public override void Draw()
        {
            spriteSheetInstance.Draw(Position, DrawEffect);

            SimpleGameEngine.Instance.spriteBatch.DrawString(ResourcesHelper.DefaultFont, Name, Position, Color.White, 0, new Vector2(0, 30), 0.65f, SpriteEffects.None, -1f);
        }

        public void HandleInput(PlayerKeyboardState playerKeyboardState)
        {
            this.playerKeyboardState = playerKeyboardState;
        }

        private void Jump()
        {
            if (IsStandingOnSolid)
            {
                Velocity = new Vector2(Velocity.X, -6f);
            }
        }

        private void Shoot()
        {
            var bullet = new Bullet(this, Position + new Vector2(30 * HorizontalDirection, 12), HorizontalDirection);
            PlatformGame.Instance.MarkGameObjectForAdd(bullet);            
        }
    }
}