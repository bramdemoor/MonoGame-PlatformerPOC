using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.Domain;
using PlatformerPOC.Events;
using PlatformerPOC.Helpers;
using PlatformerPOC.Messages;
using PlatformerPOC.Seeding;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace PlatformerPOC
{
    public class PlatformGame : Game, IAppender
    {      
        public static readonly EventAggregator eventAggregationManager = new EventAggregator();           
    
        private readonly ILog log;                

        public DebugCommandUI DebugCommandUI { get; private set; }
        
        public GameDataLoader GameDataLoader { get; private set; }
        public int RoundCounter { get; set; }
        private Editor.Editor LevelEditor { get; set; }
        public readonly FPSCounter fpsCounter;
        public readonly Renderer renderer;

        public static Random Randomizer = new Random();

        public string Name { get; set; }

        public GameMode GameMode { get; set; }

        public readonly GameWorld gameWorld;

        public SpriteFont DefaultFont
        {
            get { return GameDataLoader.DefaultFont; }
        }

        public PlatformGame()
        {       
            log4net.Config.BasicConfigurator.Configure();
            log = LogManager.GetLogger(typeof(PlatformGame));
            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetLoggerRepository()).Root.AddAppender(this);

            GameDataLoader = new GameDataLoader(this);          
            GameMode = new EliminationGameMode();
            fpsCounter = new FPSCounter();
            renderer = new Renderer(this);
            gameWorld = new GameWorld(this);

            // TODO BDM: Move to preloader
            Content.RootDirectory = "Content";            

            IsMouseVisible = Config.DebugModeEnabled;
        }

        protected override void Initialize()
        {
            log.Info("Initializing game engine...");

            base.Initialize();

            DebugCommandUI.RegisterCommand("toggle-debug", "Turn debug mode on or off", CoreCommands.ToggleDebugCommand);
            DebugCommandUI.RegisterCommand("toggle-sound", "Turn sound on or off", CoreCommands.ToggleSoundCommand);
            RegisterConsoleCommands();
        }

        protected override void LoadContent()
        {
            renderer.LoadContent();
                        
            GameDataLoader.LoadContent(Content);

            base.LoadContent();

            LevelEditor = new Editor.Editor(this);

            eventAggregationManager.AddListener(new GoreFactory(this), true);
            eventAggregationManager.AddListener(new SpawnPlayersHandler(this), true);
            eventAggregationManager.AddListener(new StartGameHandler(this), true);     
            eventAggregationManager.AddListener(new CheckGameStateHandler(this), true);
            eventAggregationManager.AddListener(new ShootHandler(this), true);
            eventAggregationManager.AddListener(new PowerupPickedUpHandler(this), true);     

            eventAggregationManager.SendMessage(new StartGameMessage());

            DebugCommandUI = new DebugCommandUI(this, DefaultFont);
            Components.Add(DebugCommandUI);
        }

        protected override void UnloadContent()
        {            
        }

        protected override void Update(GameTime gameTime)
        {
            fpsCounter.Update(gameTime);            

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            gameWorld.Update(gameTime);

            renderer.UpdateCameraPosition(gameWorld.LocalPlayer.Position.X, gameTime);

            eventAggregationManager.SendMessage(new CheckGameStateMessage());            

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
        
        public void Close()
        {
            
        }

        private void RegisterConsoleCommands()
        {
            DebugCommandUI.RegisterCommand("toggle-edit", "Turn level editor mode on or off", LevelEditor.ToggleEditModeCommand);
        }
    }
}