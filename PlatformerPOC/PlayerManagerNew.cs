using System.Collections.Generic;
using GameEngine.GameObjects;
using GameEngine.Timing;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.Control;
using PlatformerPOC.Control.AI;
using PlatformerPOC.GameObjects;
using System.Linq;

namespace PlatformerPOC
{
    public class PlayerManagerNew
    {
        private readonly PlatformGame game;

        private GameTimer hearbeatTimer;

        public List<Player> Players { get; set; }
        public Player LocalPlayer { get; private set; }

        public PlayerManagerNew(PlatformGame game)
        {
            this.game = game;

            Players = new List<Player>();
        }

        public void CreatePlayers()
        {
            LocalPlayer = new Player(game, "Player 1", 1, new GameObjectState());
            Players.Add(LocalPlayer);
            game.AddObject(LocalPlayer);

            for (int i = 2; i < 17; i++)
            {
                var name = string.Format("Player {0} [Bot]", i);
                var p = new Player(game, name, i, new GameObjectState());
                Players.Add(p);
                game.AddObject(p);                
            }

            SpawnPlayers();
        }

        public void SpawnPlayers()
        {
            for (int playerIndex = 0; playerIndex < Players.Count; playerIndex++)
            {
                var player = Players[playerIndex];
                player.Spawn(game.Level.GetSpawnPointForPlayerIndex(playerIndex + 1));
            }
        }

        public void HandleGameInput()
        {
            LocalPlayer.HandleInput(new PlayerKeyboardState(Keyboard.GetState()));
            LocalPlayer.Update();

            foreach (var player in Players)
            {
                if(player != LocalPlayer)
                {
                    player.HandleInput(new DummyAIController());
                    player.Update();                    
                }
            }
        }
    }
}