using GameEngine;
using Lidgren.Network;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.NetworkMessages;
using PlatformerPOC.Screens;

namespace PlatformerPOC
{
    /// <summary>
    /// Game-specific logic. Singleton.
    /// </summary>
    public class PlatformGame : SimpleGame
    {
        public PlayerManagerNew PlayerManager { get; private set; }

        public Level.Level Level { get; private set; }
        public ResourcesHelper ResourcesHelper { get; private set; }

        public PlatformGame()
        {
            PlayerManager = new PlayerManagerNew(this);

            ViewPort = new ViewPort(this);

            ResourcesHelper = new ResourcesHelper(this);
        }

        public override SpriteFont DefaultFont
        {
            get { return ResourcesHelper.DefaultFont; }
        }

        protected override void LoadContent()
        {
            // Important!
            base.LoadContent();

            ResourcesHelper.LoadContent(Content);

            ShowMenuScreen();
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
            Level = new Level.Level(this);

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
    }
}