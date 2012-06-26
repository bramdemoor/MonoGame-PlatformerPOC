using System.Collections.Generic;
using GameEngine;
using GameEngine.Helpers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.GameObjects;
using PlatformerPOC.Screens;

namespace PlatformerPOC
{
    /// <summary>
    /// Game-specific logic. Singleton.
    /// </summary>
    public class PlatformGame : ISimpleGame
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

        }

        public void LoadContent(ContentManager content)
        {            
            // Load resources
            Player.LoadContent(content);
            Level.LoadContent(content);
            font = content.Load<SpriteFont>("spriteFont1");

            // TODO BDM: Delegate!
            var fps = new FPSCounterComponent(SimpleGameEngine.Instance, SimpleGameEngine.Instance.spriteBatch, font);
            SimpleGameEngine.Instance.Components.Add(fps);

            ShowMenuScreen();
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
            LocalPlayer = new Player();

            SimpleGameEngine.Instance.ActiveScreen = new GameplayScreen();
        }

        public void ShowMenuScreen()
        {
            SimpleGameEngine.Instance.ActiveScreen = new LobbyScreen();
        }
    }
}