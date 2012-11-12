using System.Collections.Generic;
using GameEngine.GameObjects;
using GameEngine.Timing;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.Control;
using PlatformerPOC.Control.AI;
using PlatformerPOC.GameObjects;

namespace PlatformerPOC
{
    public class PlayerManagerNew
    {
        private readonly PlatformGame game;

        private GameTimer hearbeatTimer;

        public List<Player> Players { get; set; }
        public Player LocalPlayer { get; private set; }
        public Player DummyPlayer { get; private set; }

        public PlayerManagerNew(PlatformGame game)
        {
            this.game = game;

            Players = new List<Player>();
        }

        public void CreatePlayers()
        {
            LocalPlayer = new Player(game, "Player 1", 1, new GameObjectState());
            LocalPlayer.Spawn(game.Level.GetNextFreeSpawnPoint());

            DummyPlayer = new Player(game, "Player 2 [Bot]", 2, new GameObjectState());
            DummyPlayer.Spawn(game.Level.GetNextFreeSpawnPoint());

            // TODO BDM: Find better way of adding

            Players.Add(LocalPlayer);
            Players.Add(DummyPlayer);

            game.AddObject(LocalPlayer);
            game.AddObject(DummyPlayer);
        }

        public void HandleGameInput()
        {
            LocalPlayer.HandleInput(new PlayerKeyboardState(Keyboard.GetState()));
            LocalPlayer.Update();
            DummyPlayer.HandleInput(new DummyAIController());
            DummyPlayer.Update();
        }
    }
}