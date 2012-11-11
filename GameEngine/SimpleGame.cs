using System;
using System.Collections.Generic;
using GameEngine.GameObjects;
using GameEngine.Helpers;
using GameEngine.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace GameEngine
{   
    public interface ISimpleGame
    {
        SpriteBatch SpriteBatch { get; }
        void SwitchScreen(SimpleScreenBase screen);
    }

    /// <summary>    
    /// Manages the game infrastructure
    /// </summary>
    public class SimpleGame : Game, IAppender, ISimpleGame
    {
        private readonly List<BaseGameObject> _gameObjects;

        private readonly List<BaseGameObject> gameObjectsToAdd = new List<BaseGameObject>();
        private readonly List<BaseGameObject> gameObjectsToDelete = new List<BaseGameObject>();

        private readonly ILog log;

        private INetworkManager networkManager;
        public bool IsHost { get; private set; }

        readonly GraphicsDeviceManager graphics;

        public SpriteBatch SpriteBatch { get; private set; }

        public void SwitchScreen(SimpleScreenBase screen)
        {
            ActiveScreen = screen;
        }

        public SimpleScreenBase ActiveScreen { get; set; }

        public bool IsConnected
        {
            get { return networkManager != null && networkManager.IsConnected; }
        }

        public IEnumerable<BaseGameObject> GameObjects
        {
            get { return _gameObjects; }
        }

        public DebugDrawHelper DebugDrawHelper { get; private set; }

        public ViewPort ViewPort { get; set; }

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
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            DebugDrawHelper.LoadContent();
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

            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            ActiveScreen.Draw(gameTime);

            SpriteBatch.End();          

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
    }
}
