using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.Control;
using PlatformerPOC.Domain;
using PlatformerPOC.Domain.Gamemodes;
using PlatformerPOC.Domain.Level;
using PlatformerPOC.Drawing;
using PlatformerPOC.Events;
using PlatformerPOC.Handlers;
using PlatformerPOC.Helpers;
using PlatformerPOC.Messages;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace PlatformerPOC
{
    public class PlatformGame : Game, IAppender
    {      
        public static readonly EventAggregator eventAggregationManager = new EventAggregator();

        public ViewPort ViewPort { get; set; }
                
        //private readonly List<BaseGameObject> gameObjects;
        //private readonly List<BaseGameObject> gameObjectsToAdd = new List<BaseGameObject>();
        //private readonly List<BaseGameObject> gameObjectsToDelete = new List<BaseGameObject>();

        private readonly ILog log;                

        public DebugCommandUI DebugCommandUI { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public ResourcePreloader ResourcePreloader { get; private set; }
        public LevelManager LevelManager { get; private set; }
        public int RoundCounter { get; set; }
        private Editor.Editor LevelEditor { get; set; }
        public readonly FPSCounter fpsCounter;
        private readonly Renderer renderer;

        public readonly AIHelper _aiHelper = new AIHelper();

        public List<Player> Players { get; set; }
        public Player LocalPlayer { get; set; }

        public IEnumerable<Player> AlivePlayers
        {
            get { return Players.Where(p => p.IsAlive); }
        }

        public GameMode GameMode { get; set; }
        //public IEnumerable<BaseGameObject> GameObjects
        //{
        //    get { return gameObjects; }
        //}

        public SpriteFont DefaultFont
        {
            get { return ResourcePreloader.DefaultFont; }
        }

        public PlatformGame()
        {       
            ResourcePreloader = new ResourcePreloader(this);            
            LevelManager = new LevelManager(this);
            ViewPort = new ViewPort(this);
            GameMode = new EliminationGameMode();

            fpsCounter = new FPSCounter();

            renderer = new Renderer(this);

            log4net.Config.BasicConfigurator.Configure();
            log = LogManager.GetLogger(typeof(PlatformGame));
            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetLoggerRepository()).Root.AddAppender(this);

            Content.RootDirectory = "Content";            
            //gameObjects = new List<BaseGameObject>();

            IsMouseVisible = Config.DebugModeEnabled;

            Players = new List<Player>();
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

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            DebugCommandUI = new DebugCommandUI(this, DefaultFont);
            Components.Add(DebugCommandUI);

            ResourcePreloader.LoadContent(Content);

            LevelManager.PreloadLevels();

            base.LoadContent();

            LevelEditor = new Editor.Editor(this);

            eventAggregationManager.AddListener(new GoreFactory(this));
            eventAggregationManager.AddListener(new SpawnPlayersHandler(this));     
            eventAggregationManager.AddListener(new StartGameHandler(this));     
            eventAggregationManager.AddListener(new CheckGameStateHandler(this));     

            eventAggregationManager.SendMessage(new StartGameMessage());            
        }

        protected override void UnloadContent()
        {            
        }

        protected override void Update(GameTime gameTime)
        {
            fpsCounter.Update(gameTime);            

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ShutDown();
            }

            LocalPlayer.HandleInput(new PlayerKeyboardState(Keyboard.GetState()));

            foreach (var player in Players)
            {
                if(player.AI != null)
                {
                    player.AI.Evaluate(player.Position, LocalPlayer.Position, _aiHelper.Randomizer);
                    player.HandleInput(player.AI);
                }

                player.Update();    
            }

            //foreach (var gameObject in GameObjects)
            //{
            //    gameObject.Update(gameTime);
            //}

            // WHY: Block V scrolling
            ViewPort.ScrollTo(new Vector2(LocalPlayer.Position.X, 0));

            eventAggregationManager.SendMessage(new CheckGameStateMessage());            

            //DoHouseKeeping();

            if (Config.EditMode)
            {
                LevelEditor.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            fpsCounter.IncreaseFrames();
            renderer.Draw();
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

        private void ShutDown()
        {
            Exit();
        }

        private void RegisterConsoleCommands()
        {
            DebugCommandUI.RegisterCommand("toggle-edit", "Turn level editor mode on or off", LevelEditor.ToggleEditModeCommand);
        }

        ///// <summary>
        ///// Add object to the update/draw list
        ///// </summary>        
        //public void AddObject(BaseGameObject baseGameObject)
        //{
        //    gameObjectsToAdd.Add(baseGameObject);
        //}

        ///// <summary>
        ///// Delete object from the update/draw list
        ///// </summary>
        //public void DeleteObject(BaseGameObject baseGameObject)
        //{
        //    gameObjectsToDelete.Add(baseGameObject);
        //}

        ///// <summary>
        ///// Clean up objects to delete, and introduce new objects
        ///// </summary>
        //private void DoHouseKeeping()
        //{
        //    foreach (var baseGameObject in gameObjectsToAdd)
        //    {
        //        gameObjects.Add(baseGameObject);
        //    }

        //    gameObjectsToAdd.Clear();

        //    foreach (var baseGameObject in gameObjectsToDelete)
        //    {
        //        gameObjects.Remove(baseGameObject);
        //    }

        //    gameObjectsToDelete.Clear();
        //}
    }
}