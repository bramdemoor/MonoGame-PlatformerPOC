using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.Helpers;


namespace PlatformerPOC
{
    public class PlatformGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D playerSpriteSheetTexture;
        private Texture2D bgLayer1Texture;
        private Texture2D bgLayer2Texture;
        private Texture2D tilesetTexture;

        private SpriteSheet playerSpriteSheet;

        private int playerAnimationFrame = 0;

        SoundEffect testSound;
        SoundEffectInstance testSoundInstance;

        private Vector2 playerPos = new Vector2(100, 100);

        public PlatformGame() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferMultiSampling = true;
            graphics.IsFullScreen = false;	
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            playerSpriteSheetTexture = Content.Load<Texture2D>("player");
            bgLayer1Texture = Content.Load<Texture2D>("parallax-layer1");
            bgLayer2Texture = Content.Load<Texture2D>("parallax-layer2");
            tilesetTexture = Content.Load<Texture2D>("tileset");

            playerSpriteSheet = new SpriteSheet(playerSpriteSheetTexture);

            testSound = Content.Load<SoundEffect>("testsound");
            testSoundInstance = testSound.CreateInstance();
            testSound.Play();

            // Lame way of adding animation frames
            for (int i = 0; i < 8; i++)
            {
                playerSpriteSheet.AddSourceSprite(i, new Rectangle(i*32, 0, 32, 32));
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (testSoundInstance.State != SoundState.Playing)
            {
                testSoundInstance.Volume = 1f;

                testSoundInstance.IsLooped = false;
                testSoundInstance.Play();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                playerPos.X -= 5;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                playerPos.X += 5;
            }

            playerAnimationFrame++;
            if (playerAnimationFrame == 7)
            {
                playerAnimationFrame = 0;
            }
            
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(bgLayer1Texture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(bgLayer2Texture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Draw 1 tile
            spriteBatch.Draw(tilesetTexture, new Vector2(200, 200), new Rectangle(0, 0, 32, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Draw 1 image frame
            spriteBatch.Draw(playerSpriteSheetTexture, playerPos, playerSpriteSheet[playerAnimationFrame], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();          

            base.Draw(gameTime);
        }
    }
}
