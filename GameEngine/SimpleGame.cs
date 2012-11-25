using System;
using System.Collections.Generic;
using GameEngine.DebugHelpers;
using GameEngine.GameObjects;
using GameEngine.Graphics;
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

        private readonly GraphicsHelper graphicsHelper;

        // TODO BDM: Delegate game object management to a "GameObjectManager"
        private readonly List<BaseGameObject> gameObjects;
        private readonly List<BaseGameObject> gameObjectsToAdd = new List<BaseGameObject>();
        private readonly List<BaseGameObject> gameObjectsToDelete = new List<BaseGameObject>();

        private readonly ILog log;

        // Network
        public INetworkManager networkManager;
        public IMessageDistributor MessageDistributor;

        public bool IsHost { get; private set; }        

        public DebugCommandUI DebugCommandUI { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }

        public SimpleScreenBase ActiveScreen { get; set; }
        
        public bool IsConnected
        {
            get { return networkManager != null && networkManager.IsConnected; }
        }

        public IEnumerable<BaseGameObject> GameObjects
        {
            get { return gameObjects; }
        }

        protected SimpleGame()
        {
            log4net.Config.BasicConfigurator.Configure();
            log = LogManager.GetLogger(typeof(SimpleGame));
            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetLoggerRepository()).Root.AddAppender(this);

            Content.RootDirectory = "Content";

            graphicsHelper = new GraphicsHelper(this);

            gameObjects = new List<BaseGameObject>();

            DebugDrawHelper = new DebugDrawHelper(this);

            IsMouseVisible = CoreConfig.DebugModeEnabled;
        }

        protected override void Initialize()
        {
            // Remember: Executed BEFORE LoadContent!

            log.Info("Initializing game engine...");

            base.Initialize();

            DebugCommandUI.RegisterCommand("toggle-debug", "Turn debug mode on or off", CoreCommands.ToggleDebugCommand);
            DebugCommandUI.RegisterCommand("toggle-sound", "Turn sound on or off", CoreCommands.ToggleSoundCommand);
            RegisterConsoleCommands();
        }

        protected abstract void RegisterConsoleCommands();

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
                    //if(IsHost)
                    //{
                    //    networkManager.Send("Hello from server");    
                    //}
                    //else
                    //{
                    //    networkManager.Send("Hello from client");    
                    //}
                }

                networkManager.ReadMessages();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphicsHelper.StartDrawing();

            ActiveScreen.Draw(gameTime);

            DebugDrawHelper.DrawFps();

            graphicsHelper.EndDrawing();

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
            // Test

            Disconnect();

            Exit();
        }

        public void InitializeAsHost()
        {
            networkManager = new ServerNetworkManager(this);
            IsHost = true;
            networkManager.Connect();
        }

        public void InitializeAsClient()
        {
            networkManager = new ClientNetworkManager(this);            
            IsHost = false;
            networkManager.Connect();
        }

        public void Disconnect()
        {
            if(networkManager != null && networkManager.IsConnected)
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
                gameObjects.Add(baseGameObject);
            }

            gameObjectsToAdd.Clear();

            foreach (var baseGameObject in gameObjectsToDelete)
            {
                gameObjects.Remove(baseGameObject);
            }

            gameObjectsToDelete.Clear();
        }
    }
}
