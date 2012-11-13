using System;
using System.Collections.Generic;
using System.Globalization;
using GameEngine.Debug;
using GameEngine.GameObjects;
using GameEngine.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace GameEngine
{   
    /// <summary>    
    /// Manages the game infrastructure
    /// </summary>
    public abstract class SimpleGame : Game, IAppender
    {
        public DebugDrawHelper DebugDrawHelper { get; private set; }
        public ViewPort ViewPort { get; set; }
        public abstract SpriteFont DefaultFont { get; }

        private readonly List<BaseGameObject> _gameObjects;

        private readonly List<BaseGameObject> gameObjectsToAdd = new List<BaseGameObject>();
        private readonly List<BaseGameObject> gameObjectsToDelete = new List<BaseGameObject>();

        private readonly ILog log;

        private INetworkManager networkManager;
        public bool IsHost { get; private set; }

        readonly GraphicsDeviceManager graphics;

        //public DebugManager DebugManager { get; private set; }

        public DebugCommandUI DebugCommandUI { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }

        public SimpleScreenBase ActiveScreen { get; set; }
        
        public bool IsConnected
        {
            get { return networkManager != null && networkManager.IsConnected; }
        }

        public IEnumerable<BaseGameObject> GameObjects
        {
            get { return _gameObjects; }
        }

        public SimpleGame()
        {
            log4net.Config.BasicConfigurator.Configure();
            log = LogManager.GetLogger(typeof(SimpleGame));
            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetLoggerRepository()).Root.AddAppender(this);

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferMultiSampling = true;
            graphics.IsFullScreen = false;

            _gameObjects = new List<BaseGameObject>();

            DebugDrawHelper = new DebugDrawHelper(this);
        }

        protected override void Initialize()
        {
            // Remember: Executed BEFORE LoadContent!

            log.Info("Initializing game engine...");

            base.Initialize();

            DebugCommandUI.RegisterCommand("pos", "set position", PosCommand);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            DebugDrawHelper.LoadContent();

            DebugCommandUI = new DebugCommandUI(this, DefaultFont);
            Components.Add(DebugCommandUI);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {   
            DebugDrawHelper.Update(gameTime);
       
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

            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            ActiveScreen.Draw(gameTime);

            DebugDrawHelper.DrawFps();

            SpriteBatch.End();          

            base.Draw(gameTime);
        }

        public void SwitchScreen(SimpleScreenBase screen)
        {
            ActiveScreen = screen;
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

        /// <summary>
        /// Add object to the update/draw list
        /// </summary>        
        public void AddObject(BaseGameObject baseGameObject)
        {
            gameObjectsToAdd.Add(baseGameObject);
        }

        /// <summary>
        /// Delete object from the update/draw list
        /// </summary>
        public void DeleteObject(BaseGameObject baseGameObject)
        {
            gameObjectsToDelete.Add(baseGameObject);
        }

        /// <summary>
        /// Clean up objects to delete, and introduce new objects
        /// </summary>
        public void DoHouseKeeping()
        {
            foreach (var baseGameObject in gameObjectsToAdd)
            {
                _gameObjects.Add(baseGameObject);
            }

            gameObjectsToAdd.Clear();

            foreach (var baseGameObject in gameObjectsToDelete)
            {
                _gameObjects.Remove(baseGameObject);
            }

            gameObjectsToDelete.Clear();
        }

        // Position for debug command test.
        Vector2 debugPos = new Vector2(100, 100);


        /// <summary>
        /// This method is called from DebugCommandHost when the user types the 'pos'
        /// command into the command prompt. This is registered with the command prompt
        /// through the DebugCommandUI.RegisterCommand method we called in Initialize.
        /// </summary>
        void PosCommand(IDebugCommandHost host, string command, IList<string> arguments)
        {
            // if we got two arguments from the command
            if (arguments.Count == 2)
            {
                // process text "pos xPos yPos" by parsing our two arguments
                debugPos.X = Single.Parse(arguments[0], CultureInfo.InvariantCulture);
                debugPos.Y = Single.Parse(arguments[1], CultureInfo.InvariantCulture);
            }
            else
            {
                // if we didn't get two arguments, we echo the current position of the cat
                host.Echo(String.Format("Pos={0},{1}", debugPos.X, debugPos.Y));
            }
        }
    }
}
