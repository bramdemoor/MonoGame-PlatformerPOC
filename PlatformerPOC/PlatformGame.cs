using System.Collections.Generic;
using GameEngine;
using GameEngine.GameObjects;
using GameEngine.Helpers;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.GameObjects;
using PlatformerPOC.NetworkMessages;
using PlatformerPOC.Screens;

namespace PlatformerPOC
{
    /// <summary>
    /// Game-specific logic. Singleton.
    /// </summary>
    public class PlatformGame : SimpleGameBase
    {
        public List<Player> Players { get; set; }
        public Player LocalPlayer { get; private set; }
        public Level Level { get; private set; }

        public SpriteFont font { get; private set; }

        public static PlatformGame Instance { get; private set; }

        public static void Start()
        {
            Instance = new PlatformGame();
            SimpleGameEngine.InitializeEngine(Instance);
            SimpleGameEngine.Instance.Run();
        }

        private PlatformGame()
        {
            Players = new List<Player>();
        }

        public override void LoadContent(ContentManager content)
        {
            ResourcesHelper.LoadContent(content);

            font = content.Load<SpriteFont>("spriteFont1");

            // TODO BDM: Delegate!
            var fps = new FPSCounterComponent(SimpleGameEngine.Instance, SimpleGameEngine.Instance.spriteBatch, font);
            SimpleGameEngine.Instance.Components.Add(fps);

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

            // Naive try
            PlatformGame.Instance.StartGame();
        }

        public void StartGame()
        {
            Level = new Level();

            LocalPlayer = new Player("Player 1", 1, new GameObjectState());

            var otherPlayer = new Player("Player 2", 2, new GameObjectState());

            // TODO BDM: Find better way of adding

            Players.Add(LocalPlayer);
            Players.Add(otherPlayer);

            MarkGameObjectForAdd(LocalPlayer);
            MarkGameObjectForAdd(otherPlayer);

            SimpleGameEngine.Instance.ActiveScreen = new GameplayScreen();
        }

        public void ShowMenuScreen()
        {
            SimpleGameEngine.Instance.ActiveScreen = new LobbyScreen();
        }
    }
}