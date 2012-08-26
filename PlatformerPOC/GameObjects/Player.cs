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

        public string Name { get; set; }

        public int Life { get; set; }

        public bool IsAlive
        {
            get { return Life > 0; }
        }

        private readonly CustomSpriteSheetInstance spriteSheetInstance;
        
        private PlayerKeyboardState playerKeyboardState;

        public Rectangle RectangleCollisionBounds { get { return new Rectangle((int) Position.X,(int) Position.Y,32,32); } }

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

            ApplyGravity();

            if (IsMovingHorizontally)
            {
                spriteSheetInstance.LoopWithReverse();
            }            
        }

        private void ApplyInput()
        {
            if (playerKeyboardState.IsMoveLeftPressed)
            {
                MoveLeft();
            }

            if (playerKeyboardState.IsMoveRightPressed)
            {
                MoveRight();
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

        private void ApplyGravity()
        {
            if(!PlatformGame.Instance.Level.IsGroundBelow(Position))
            {
                Velocity += new Vector2(0, 0.2f);                
            }
            else
            {
                if (Velocity.Y > 0) Velocity = Vector2.Zero;
            }

            Position = new Vector2(Position.X, Position.Y + Velocity.Y);
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
            if (PlatformGame.Instance.Level.IsGroundBelow(Position))
            {
                Velocity = new Vector2(0, -6f);
            }
        }

        private void Shoot()
        {
            var bullet = new Bullet(this, Position + new Vector2(30 * HorizontalDirection, 12), HorizontalDirection);
            PlatformGame.Instance.MarkGameObjectForAdd(bullet);            
        }

        private void MoveLeft()
        {
            if(!PlatformGame.Instance.Level.IsInBoundsLeft(Position)) return;

            Position = new Vector2(Position.X - 5, Position.Y);

            HorizontalDirection = -1;
        }

        private void MoveRight()
        {
            if (!PlatformGame.Instance.Level.IsInBoundsRight(Position)) return;

            Position = new Vector2(Position.X + 5, Position.Y);

            HorizontalDirection = 1;
        }
    }
}