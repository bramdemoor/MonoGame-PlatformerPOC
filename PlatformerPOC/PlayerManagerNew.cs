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
            LocalPlayer.Spawn(game.Level.GetSpawnPointForPlayerIndex(1));

            for (int i = 2; i < 4; i++)
            {
                var name = string.Format("Player {0} [Bot]", i);
                var p = new Player(game, name, i, new GameObjectState());
                p.Spawn(game.Level.GetSpawnPointForPlayerIndex(i));
                Players.Add(p);
                game.AddObject(p);                
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