using System.Collections.Generic;
using GameEngine;
using GameEngine.GameObjects;
using Lidgren.Network;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.GameObjects;
using PlatformerPOC.NetworkMessages;
using PlatformerPOC.Screens;

namespace PlatformerPOC
{
    /// <summary>
    /// Game-specific logic. Singleton.
    /// </summary>
    public class PlatformGame : SimpleGame
    {
        public List<Player> Players { get; set; }
        public Player LocalPlayer { get; private set; }
        public Player DummyPlayer { get; private set; }
        public Level.Level Level { get; private set; }
        public ResourcesHelper ResourcesHelper { get; private set; }

        public PlatformGame()
        {
            Players = new List<Player>();

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

            LocalPlayer = new Player(this, "Player 1", 1, new GameObjectState());
            LocalPlayer.Spawn(Level.GetNextFreeSpawnPoint());

            DummyPlayer = new Player(this, "Player 2 [Bot]", 2, new GameObjectState());
            DummyPlayer.Spawn(Level.GetNextFreeSpawnPoint());

            // TODO BDM: Find better way of adding

            Players.Add(LocalPlayer);
            Players.Add(DummyPlayer);

            AddObject(LocalPlayer);
            AddObject(DummyPlayer);

            SwitchScreen(new GameplayScreen(this));
        }

        public void ShowMenuScreen()
        {
            SwitchScreen(new LobbyScreen(this));
        }

        public void GeneralUpdate()
        {
            ViewPort.ScrollTo(LocalPlayer.Position);
        }
    }
}