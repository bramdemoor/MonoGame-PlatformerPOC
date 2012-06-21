using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using PlatformerPOC.Domain;
using PlatformerPOC.Helpers;
using PlatformerPOC.Network;
using log4net;
using log4net.Appender;
using log4net.Core;


namespace PlatformerPOC
{
    // Logging based on http://weblogs.asp.net/psteele/archive/2010/01/25/live-capture-of-log4net-logging.aspx

    public class PlatformGame : Game, IAppender
    {
        private ILog log;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        
        private Texture2D bgLayer1Texture;
        private Texture2D bgLayer2Texture;
        private Texture2D tilesetTexture;

        SoundEffect testSound;
        SoundEffectInstance testSoundInstance;

        private INetworkManager networkManager;

        private Player player;

        public PlatformGame()
        {
            log4net.Config.BasicConfigurator.Configure();
            log = LogManager.GetLogger(typeof(PlatformGame));

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferMultiSampling = true;
            graphics.IsFullScreen = false;	
        }

        protected override void Initialize()
        {
            log.Info("Initializing game...");

            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetLoggerRepository()).Root.AddAppender(this);

            log.Info("Press H to host and J to join (localhost test only)");

            player = new Player();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Player.LoadContent(Content);

            bgLayer1Texture = Content.Load<Texture2D>("parallax-layer1");
            bgLayer2Texture = Content.Load<Texture2D>("parallax-layer2");
            tilesetTexture = Content.Load<Texture2D>("tileset");

            testSound = Content.Load<SoundEffect>("testsound");
            testSoundInstance = testSound.CreateInstance();
            testSound.Play();
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
                player.MoveLeft();                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                player.MoveRight();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                networkManager = new ServerNetworkManager();
                networkManager.Connect();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                networkManager = new ClientNetworkManager();
                networkManager.Connect();
            }

            player.Update();


            if (networkManager != null)
            {
                if (networkManager.IsConnected)
                {
                    networkManager.Send("I'm a client or a server");
                    
                }

                networkManager.ReadMessages();
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
            player.Draw(spriteBatch);            

            spriteBatch.End();          

            base.Draw(gameTime);
        }

        public void Close()
        {
            //
        }

        public void DoAppend(LoggingEvent loggingEvent)
        {
            Console.WriteLine("{2}: {0}: {1}\r\n", loggingEvent.Level.Name, loggingEvent.MessageObject, loggingEvent.LoggerName);
        }

        public string Name { get; set; }
    }
}
