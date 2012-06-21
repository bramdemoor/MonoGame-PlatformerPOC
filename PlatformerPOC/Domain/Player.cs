using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Domain
{
    public class Player
    {
        // Same for all players
        private static Texture2D spriteSheetTexture;
        private static SpriteSheet spriteSheet;
        private static SoundEffect spawnSound;

        private SoundEffectInstance spawnSoundInstance;

        private int animationFrame = 0;
        private Vector2 position = new Vector2(100, 100);

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

        public Player()
        {
            Spawn();
        }

        public void Spawn()
        {
            spawnSoundInstance = spawnSound.CreateInstance();
            spawnSoundInstance.Play();            
        }

        public void Update()
        {
            if (spawnSoundInstance.State != SoundState.Playing)
            {
                spawnSoundInstance.Volume = 1f;

                spawnSoundInstance.IsLooped = false;
                spawnSoundInstance.Play();
            }

            animationFrame++;
            if (animationFrame == 7)
            {
                animationFrame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheetTexture, position, spriteSheet[animationFrame], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void HandleInput(PlayerKeyboardState playerKeyboardState)
        {
            if (playerKeyboardState.IsMoveLeftPressed)
            {
                MoveLeft();
            }

            if (playerKeyboardState.IsMoveRightPressed)
            {
                MoveRight();
            }
        }

        private void MoveLeft()
        {
            position.X -= 5;
        }

        private void MoveRight()
        {
            position.X += 5;
        }
    }
}