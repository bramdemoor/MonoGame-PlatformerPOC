using GameEngine;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Concept;
using PlatformerPOC.Level;
using PlatformerPOC.Network;
using PlatformerPOC.Network.Messages;
using PlatformerPOC.Screens;

namespace PlatformerPOC
{
    /// <summary>
    /// Game-specific logic.
    /// </summary>
    public class PlatformGame : SimpleGame
    {
        public PlayerManagerNew PlayerManager { get; private set; }

        public ResourcesHelper ResourcesHelper { get; private set; }

        public LevelManager LevelManager { get; private set; }

        public int RoundCounter { get; set; }

        public Editor.Editor LevelEditor { get; set; }

        public GameMode GameMode { get; set; }

        public PlatformGame()
        {
            MessageDistributor = new MessageDistributor(this);
            ResourcesHelper = new ResourcesHelper(this);
            PlayerManager = new PlayerManagerNew(this);
            LevelManager = new LevelManager(this);
            ViewPort = new ViewPort(this);
            GameMode = new TeamEliminationGameMode();
        }

        public override SpriteFont DefaultFont
        {
            get { return ResourcesHelper.DefaultFont; }
        }

        protected override void LoadContent()
        {            
            ResourcesHelper.LoadContent(Content);

            LevelManager.PreloadLevels();

            // Important!
            base.LoadContent();

            LevelEditor = new Editor.Editor(this);

            ShowMenuScreen();
            //StartGame();
        }

        protected override void RegisterConsoleCommands()
        {
            DebugCommandUI.RegisterCommand("toggle-edit", "Turn level editor mode on or off", LevelEditor.ToggleEditModeCommand);
        }

        private void HandleUpdatePlayerStateMessage(NetIncomingMessage im)
        {
            var message = new UpdatePlayerStateMessage(im);

            //var timeDelay = (float)(NetTime.Now - im.SenderConnection.GetLocalTime(message.MessageTime));

            //Player player = this.playerManager.GetPlayer(message.Id)
            //                ??
            //                this.playerManager.AddPlayer(
            //                    message.Id, message.Position, message.Velocity, message.Rotation, false);

            //if (player.LastUpdateTime < message.MessageTime)
            //{
            //    player.SimulationState.Position = message.Position += message.Velocity * timeDelay;
            //    player.SimulationState.Velocity = message.Velocity;
            //    player.SimulationState.Rotation = message.Rotation;

            //    player.LastUpdateTime = message.MessageTime;
            //}
        }

        public void HostStartGame()
        {
            if(!IsHost) return;

            networkManager.Send(new HostStartGameMessage());
            
            StartGame();
        }

        public void StartGame()
        {
            RoundCounter = 1;

            LevelManager.StartLevel();

            PlayerManager.CreatePlayers();

            SwitchScreen(new GameplayScreen(this));
        }

        public void ShowMenuScreen()
        {
            SwitchScreen(new LobbyScreen(this));
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
    }
}