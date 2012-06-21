using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.Domain;
using PlatformerPOC.Helpers;
using PlatformerPOC.Network;
using PlatformerPOC.Screens;
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

        public SpriteBatch spriteBatch { get; private set; }

        private INetworkManager networkManager;

        public Player player { get; private set; }
        public Domain.Level level { get; private set; }

        public bool IsHost { get; private set; }
        private FPSCounterComponent fps;

        public SpriteFont font { get; private set; }

        private SimpleScreen activeScreen;

        public bool IsConnected
        {
            get { return networkManager != null && networkManager.IsConnected; }
        }

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load resources
            Player.LoadContent(Content);
            Domain.Level.LoadContent(Content);
            font = Content.Load<SpriteFont>("spriteFont1");
             
            fps = new FPSCounterComponent(this, spriteBatch, font);
            Components.Add(fps);

            ShowMenuScreen();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {             
            activeScreen.Update(gameTime);

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

        public void ShowMenuScreen()
        {
            activeScreen = new LobbyScreen(this);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            activeScreen.Draw(gameTime);

            spriteBatch.End();          

            base.Draw(gameTime);
        }

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

        public void StartGame()
        {
            level = new Domain.Level();
            player = new Player();

            activeScreen = new GameplayScreen(this);
        }

        public void Disconnect()
        {
            networkManager.Disconnect();
        }

        public void HostStartGame()
        {
            // TODO BDM: Check if really host, exc otherwise

            // Naive try
            StartGame();
        }
    }
}
