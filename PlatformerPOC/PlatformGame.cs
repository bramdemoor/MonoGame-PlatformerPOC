using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.Domain;
using PlatformerPOC.Domain.Gamemodes;
using PlatformerPOC.Domain.Level;
using PlatformerPOC.Drawing;
using PlatformerPOC.Helpers;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace PlatformerPOC
{
    public class PlatformGame : Game, IAppender
    {         
        public ViewPort ViewPort { get; set; }
                
        private readonly List<BaseGameObject> gameObjects;
        private readonly List<BaseGameObject> gameObjectsToAdd = new List<BaseGameObject>();
        private readonly List<BaseGameObject> gameObjectsToDelete = new List<BaseGameObject>();

        private readonly ILog log;                

        public DebugCommandUI DebugCommandUI { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public PlayerManagerNew PlayerManager { get; private set; }
        public ResourcePreloader ResourcePreloader { get; private set; }
        public LevelManager LevelManager { get; private set; }
        public int RoundCounter { get; set; }
        public Editor.Editor LevelEditor { get; set; }
        public readonly FPSCounter fpsCounter;
        private Renderer renderer;

        public GameMode GameMode { get; set; }
        public IEnumerable<BaseGameObject> GameObjects
        {
            get { return gameObjects; }
        }

        public SpriteFont DefaultFont
        {
            get { return ResourcePreloader.DefaultFont; }
        }

        public PlatformGame()
        {
            ResourcePreloader = new ResourcePreloader(this);
            PlayerManager = new PlayerManagerNew(this);
            LevelManager = new LevelManager(this);
            ViewPort = new ViewPort(this);
            GameMode = new EliminationGameMode();

            fpsCounter = new FPSCounter();

            renderer = new Renderer(this);

            log4net.Config.BasicConfigurator.Configure();
            log = LogManager.GetLogger(typeof(PlatformGame));
            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetLoggerRepository()).Root.AddAppender(this);

            Content.RootDirectory = "Content";            
            gameObjects = new List<BaseGameObject>();

            IsMouseVisible = Config.DebugModeEnabled;
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

            // Important!
            base.LoadContent();

            LevelEditor = new Editor.Editor(this);

            StartGame();
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

            PlayerManager.HandleGameInput();

            foreach (var gameObject in GameObjects)
            {
                gameObject.Update(gameTime);
            }

            GeneralUpdate();

            CheckGameState();

            DoHouseKeeping();

            if (Config.EditMode)
            {
                LevelEditor.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            renderer.Draw();
            base.Draw(gameTime);
        }

        private void CheckGameState()
        {
            switch (PlayerManager.AlivePlayers.Count())
            {
                case 0:
                    StartNextRound();
                    break;
                case 1:
                    // Only 1 player? Don't do the checks.
                    if (PlayerManager.Players.Count() == 1) return;

                    var winner = PlayerManager.AlivePlayers.Single();
                    winner.Score.MarkWin();
                    StartNextRound();

                    break;
                default:
                    // continue game
                    break;
            }
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

        protected void RegisterConsoleCommands()
        {
            DebugCommandUI.RegisterCommand("toggle-edit", "Turn level editor mode on or off", LevelEditor.ToggleEditModeCommand);
        }

        public void StartGame()
        {
            RoundCounter = 1;
            LevelManager.StartLevel();
            PlayerManager.CreatePlayers();
        }        

        public void GeneralUpdate()
        {
            var pos = PlayerManager.LocalPlayer.Position;

            // WHY: Block V scrolling
            ViewPort.ScrollTo(new Vector2(pos.X, 0));
        }

        public void StartNextRound()
        {
            PlayerManager.SpawnPlayers();

            RoundCounter++;
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