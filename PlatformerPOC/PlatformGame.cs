﻿using GameEngine;
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
            GameMode = new EliminationGameMode();
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

            //ShowMenuScreen();
            StartGame();
        }

        protected override void RegisterConsoleCommands()
        {
            DebugCommandUI.RegisterCommand("toggle-edit", "Turn level editor mode on or off", LevelEditor.ToggleEditModeCommand);
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