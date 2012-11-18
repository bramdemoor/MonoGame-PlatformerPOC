using GameEngine;
using Lidgren.Network;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Level;
using PlatformerPOC.NetworkMessages;
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

        public PlatformGame()
        {
            PlayerManager = new PlayerManagerNew(this);

            LevelManager = new LevelManager(this);

            ViewPort = new ViewPort(this);

            ResourcesHelper = new ResourcesHelper(this);
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
            // TODO BDM: Check if really host, exc otherwise

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
            ViewPort.ScrollTo(PlayerManager.LocalPlayer.Position);
        }

        public void StartNextRound()
        {
            PlayerManager.SpawnPlayers();

            RoundCounter++;
        }
    }
}