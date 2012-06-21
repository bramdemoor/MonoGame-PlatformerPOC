using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.Domain;
using PlatformerPOC.Network;
using log4net;
using log4net.Appender;
using log4net.Core;


namespace PlatformerPOC
{
    // Logging based on http://weblogs.asp.net/psteele/archive/2010/01/25/live-capture-of-log4net-logging.aspx

    public class PlatformGame : Game, IAppender
    {
        private readonly ILog log;

        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private INetworkManager networkManager;

        private Player player;
        private Domain.Level level;

        public PlatformGame()
        {
            log4net.Config.BasicConfigurator.Configure();
            log = LogManager.GetLogger(typeof(PlatformGame));
            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetLoggerRepository()).Root.AddAppender(this);

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferMultiSampling = true;
            graphics.IsFullScreen = false;	
        }

        protected override void Initialize()
        {
            // Remember: Executed BEFORE LoadContent!

            log.Info("Initializing game...");            
            log.Info("Press H to host and J to join (localhost test only)");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Player.LoadContent(Content);
            Domain.Level.LoadContent(Content);

            // Start game!
            level = new Domain.Level();
            player = new Player();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {


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

            level.Draw(spriteBatch);
            
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
