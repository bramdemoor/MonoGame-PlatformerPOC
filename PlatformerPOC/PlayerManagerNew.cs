using System.Collections.Generic;
using GameEngine.GameObjects;
using GameEngine.Timing;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.Concept;
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

        private AINameHelper AINameHelper;

        public List<Player> Players { get; set; }
        public Player LocalPlayer { get; private set; }

        public IEnumerable<Player> AlivePlayers
        {
            get { return Players.Where(p => p.IsAlive); }
        }

        public PlayerManagerNew(PlatformGame game)
        {
            this.game = game;

            Players = new List<Player>();

            AINameHelper = new AINameHelper();
        }

        public void CreatePlayers()
        {
            AINameHelper.Reset();

            if(game.GameMode is EliminationGameMode)
            {
                AddLocalPlayer(Team.Neutral);

                for (int i = 2; i < 17; i++)
                {
                    AddBot(i, Team.Neutral);
                }                
            }
            else
            {
                AddLocalPlayer(Team.Red);

                for (int i = 2; i < 9; i++)
                {
                    AddBot(i,Team.Red);
                }
                for (int i = 9; i < 17; i++)
                {
                    AddBot(i, Team.Blue);
                }  
            }

            SpawnPlayers();
        }

        private void AddLocalPlayer(Team team)
        {
            LocalPlayer = new Player(game, "Player 1", 1, new GameObjectState());
            LocalPlayer.SwitchTeam(team);
            Players.Add(LocalPlayer);
            game.AddObject(LocalPlayer);
        }

        private void AddBot(int i, Team team)
        {
            var botPlayer = new Player(game, string.Format("{0} [Bot]", AINameHelper.GetRandomName()), i, new GameObjectState());
            botPlayer.SwitchTeam(team);
            Players.Add(botPlayer);
            game.AddObject(botPlayer);
        }

        public void SpawnPlayers()
        {
            for (int playerIndex = 0; playerIndex < Players.Count; playerIndex++)
            {
                var player = Players[playerIndex];
                player.Spawn(game.LevelManager.CurrentLevel.GetSpawnPointForPlayerIndex(playerIndex + 1));
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
                    player.HandleInput(new DummyAIController(player.Position, LocalPlayer.Position));
                    player.Update();                    
                }
            }
        }

        public void AddPlayer(string name)
        {
            Players.Add(new Player(game, name, 989, null));
        }
    }
}