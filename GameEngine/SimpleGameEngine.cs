using System;
using GameEngine.Helpers;
using GameEngine.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace GameEngine
{   
    /// <summary>    
    /// Manages the game infrastructure. Singleton.
    /// </summary>
    public class SimpleGameEngine : Game, IAppender
    {
        private readonly ILog log;

        private INetworkManager networkManager;
        public bool IsHost { get; private set; }

        readonly GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch { get; private set; }

        public SimpleScreenBase ActiveScreen { get; set; }

        public bool IsConnected
        {
            get { return networkManager != null && networkManager.IsConnected; }
        }

        public static SimpleGameEngine Instance { get; private set; }

        public SimpleGameBase Game { get; private set; }

        public static void InitializeEngine(SimpleGameBase game)
        {
            Instance = new SimpleGameEngine(game);
        }

        private SimpleGameEngine(SimpleGameBase game)
        {
            this.Game = game;

            log4net.Config.BasicConfigurator.Configure();
            log = LogManager.GetLogger(typeof(SimpleGameEngine));
            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetLoggerRepository()).Root.AddAppender(this);

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferMultiSampling = true;
            graphics.IsFullScreen = false;	
        }

        protected override void Initialize()
        {
            // Remember: Executed BEFORE LoadContent!

            log.Info("Initializing game engine...");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Game.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {             
            ActiveScreen.Update(gameTime);

            if (networkManager != null)
            {
                // WHY: Network test
                if (networkManager.IsConnected)
                {
                    if(IsHost)
                    {
                        networkManager.Send("Hello from server");    
                    }
                    else
                    {
                        networkManager.Send("Hello from client");    
                    }
                }

                networkManager.ReadMessages();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            ActiveScreen.Draw(gameTime);

            spriteBatch.End();          

            base.Draw(gameTime);
        }

        /// <summary>
        /// Logging based on http://weblogs.asp.net/psteele/archive/2010/01/25/live-capture-of-log4net-logging.aspx
        /// </summary>        
        public void DoAppend(LoggingEvent loggingEvent)
        {
            Console.WriteLine("{2}: {0}: {1}\r\n", loggingEvent.Level.Name, loggingEvent.MessageObject, loggingEvent.LoggerName);
        }

        public string Name { get; set; }

        public void Close() { }

        public void ShutDown()
        {
            Exit();
        }

        public void InitializeAsHost()
        {
            networkManager = new ServerNetworkManager();
            IsHost = true;
            networkManager.Connect();
        }

        public void InitializeAsClient()
        {
            networkManager = new ClientNetworkManager();
            IsHost = false;
            networkManager.Connect();
        }

        public void Disconnect()
        {
            networkManager.Disconnect();
        }
    }
}
