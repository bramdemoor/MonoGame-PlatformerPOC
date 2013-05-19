using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.Concept;
using PlatformerPOC.Concept.Gamemodes;
using PlatformerPOC.Concept.Teams;
using PlatformerPOC.Control;
using PlatformerPOC.Control.AI;
using PlatformerPOC.GameObjects;
using System.Linq;
using PlatformerPOC.Helpers;

namespace PlatformerPOC
{
    public class PlayerManagerNew
    {
        private readonly PlatformGame game;

        private GameTimer hearbeatTimer;

        private readonly AIHelper _aiHelper;

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

            _aiHelper = new AIHelper();
        }

        public void CreatePlayers()
        {
            _aiHelper.Reset();

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
            LocalPlayer = new Player(game, "Player 1", 1, new GameObjectState(), game.ResourcesHelper.Characters.First());
            LocalPlayer.SwitchTeam(team);
            Players.Add(LocalPlayer);
            game.AddObject(LocalPlayer);
        }

        private void AddBot(int i, Team team)
        {
            var botPlayer = new Player(game, string.Format("{0} [Bot]", _aiHelper.GetRandomName()), i, null, game.ResourcesHelper.GetRandomCharacter());
            botPlayer.SwitchTeam(team);
            botPlayer.AI = new DummyAIController();            
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

            foreach (var player in Players)
            {
                if(player.AI != null)
                {
                    player.AI.Evaluate(player.Position, LocalPlayer.Position, _aiHelper.Randomizer);
                    player.HandleInput(player.AI);
                }

                player.Update();    
            }
        }

        public void AddPlayer(string name)
        {
            Players.Add(new Player(game, name, 989, null, game.ResourcesHelper.Characters.First()));
        }
    }
}