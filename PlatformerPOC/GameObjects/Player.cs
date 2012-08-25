using GameEngine;
using GameEngine.GameObjects;
using GameEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.GameObjects
{
    public class Player : BaseGameObject 
    {
        public string Name { get; set; }

        // Same for all players
        private static Texture2D spriteSheetTexture;
        private static SpriteSheet spriteSheet;
        private static SoundEffect spawnSound;

        private SoundEffectInstance spawnSoundInstance;

        private int animationFrame = 0;

        private int horizontalDirection = 1;

        private PlayerKeyboardState playerKeyboardState;

        public Rectangle RectangleCollisionBounds { get { return new Rectangle((int) Position.X,(int) Position.Y,32,32); } }

        public static void LoadContent(ContentManager content)
        {
            spriteSheetTexture = content.Load<Texture2D>("player");
            spriteSheet = new SpriteSheet(spriteSheetTexture);

            // Lame way of adding animation frames
            for (int i = 0; i < 8; i++)
            {
                spriteSheet.AddSourceSprite(i, new Rectangle(i * 32, 0, 32, 32));
            }

            spawnSound = content.Load<SoundEffect>("testsound");
        }

        public Player(string name, long id, GameObjectState gameObjectState)
        {
            this.Name = name;

            Spawn();
        }

        public void Spawn()
        {
            //spawnSoundInstance = spawnSound.CreateInstance();
            //spawnSoundInstance.Play();
            Position = PlatformGame.Instance.Level.GetNextFreeSpawnPoint();
        }

        public void Update()
        {
            ApplyMovement();

            ApplyGravity();

            //if (spawnSoundInstance.State != SoundState.Playing)
            //{
            //    spawnSoundInstance.Volume = 1f;

            //    spawnSoundInstance.IsLooped = false;
            //    spawnSoundInstance.Play();
            //}

            animationFrame++;
            if (animationFrame == 7)
            {
                animationFrame = 0;
            }
        }

        private void ApplyMovement()
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
            var drawEffect = horizontalDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            SimpleGameEngine.Instance.spriteBatch.Draw(spriteSheetTexture, Position, spriteSheet[animationFrame], Color.White, 0f, Vector2.Zero, 1f, drawEffect, 0f);
            SimpleGameEngine.Instance.spriteBatch.DrawString(PlatformGame.Instance.font, Name, Position, Color.White, 0, new Vector2(0, 30), 0.65f, SpriteEffects.None, -1f);
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
            var bullet = new Bullet(this, Position + new Vector2(30 * horizontalDirection, 12), horizontalDirection);
            PlatformGame.Instance.MarkGameObjectForAdd(bullet);            
        }

        private void MoveLeft()
        {
            if(!PlatformGame.Instance.Level.IsInBoundsLeft(Position)) return;

            Position = new Vector2(Position.X - 5, Position.Y);

            horizontalDirection = -1;
        }

        private void MoveRight()
        {
            if (!PlatformGame.Instance.Level.IsInBoundsRight(Position)) return;

            Position = new Vector2(Position.X + 5, Position.Y);

            horizontalDirection = 1;
        }
    }
}