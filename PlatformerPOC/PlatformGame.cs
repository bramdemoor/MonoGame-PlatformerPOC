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
using PlatformerPOC.Domain.Teams;
using PlatformerPOC.Drawing;
using PlatformerPOC.Events;
using PlatformerPOC.Helpers;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace PlatformerPOC
{
    public class PlatformGame : Game, IAppender
    {      
        public static readonly EventAggregator eventAggregationManager = new EventAggregator();

        public ViewPort ViewPort { get; set; }
                
        private readonly List<BaseGameObject> gameObjects;
        private readonly List<BaseGameObject> gameObjectsToAdd = new List<BaseGameObject>();
        private readonly List<BaseGameObject> gameObjectsToDelete = new List<BaseGameObject>();

        private readonly ILog log;                

        public DebugCommandUI DebugCommandUI { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public ResourcePreloader ResourcePreloader { get; private set; }
        public LevelManager LevelManager { get; private set; }
        public int RoundCounter { get; set; }
        public Editor.Editor LevelEditor { get; set; }
        public readonly FPSCounter fpsCounter;
        private Renderer renderer;

        private AIHelper _aiHelper = new AIHelper();

        public List<Player> Players { get; set; }
        public Player LocalPlayer { get; private set; }

        public IEnumerable<Player> AlivePlayers
        {
            get { return Players.Where(p => p.IsAlive); }
        }

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
            eventAggregationManager.AddListener(new GoreFactory(this));            

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
            gameObjects = new List<BaseGameObject>();

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

            HandleGameInput();

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
            fpsCounter.IncreaseFrames();
            renderer.Draw();
            base.Draw(gameTime);
        }

        private void CheckGameState()
        {
            switch (AlivePlayers.Count())
            {
                case 0:
                    StartNextRound();
                    break;
                case 1:
                    // Only 1 player? Don't do the checks.
                    if (Players.Count() == 1) return;

                    var winner = AlivePlayers.Single();
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
            CreatePlayers();
        }        

        public void GeneralUpdate()
        {
            var pos = LocalPlayer.Position;

            // WHY: Block V scrolling
            ViewPort.ScrollTo(new Vector2(pos.X, 0));
        }

        public void StartNextRound()
        {
            SpawnPlayers();

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

        public void CreatePlayers()
        {
            _aiHelper.Reset();

            if(GameMode is EliminationGameMode)
            {
                AddLocalPlayer(Team.Neutral);

                for (int i = 2; i < 4; i++)
                {
                    AddBot(i, Team.Neutral);
                }                
            }
            else
            {
                AddLocalPlayer(Team.Red);

                for (int i = 2; i < 9; i++)
                {
                    AddBot(i,Team.Red);
                }
                for (int i = 9; i < 17; i++)
                {
                    AddBot(i, Team.Blue);
                }  
            }

            SpawnPlayers();
        }

        private void AddLocalPlayer(Team team)
        {
            LocalPlayer = new Player(this, "Player 1", ResourcePreloader.Character1Sheet);
            LocalPlayer.SwitchTeam(team);
            Players.Add(LocalPlayer);
            AddObject(LocalPlayer);
        }

        private void AddBot(int i, Team team)
        {
            var botPlayer = new Player(this, string.Format("{0} [Bot]", _aiHelper.GetRandomName()), ResourcePreloader.Character2Sheet);
            botPlayer.SwitchTeam(team);
            botPlayer.AI = new DummyAIController();            
            Players.Add(botPlayer);
            AddObject(botPlayer);
        }

        public void SpawnPlayers()
        {
            for (int playerIndex = 0; playerIndex < Players.Count; playerIndex++)
            {
                var player = Players[playerIndex];
                player.Spawn(LevelManager.CurrentLevel.GetSpawnPointForPlayerIndex(playerIndex + 1));
            }
        }

        public void HandleGameInput()
        {
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
        }
    }
}