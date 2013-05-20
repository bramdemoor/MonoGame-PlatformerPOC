using System.Collections.Generic;
using System.Linq;
using PlatformerPOC.AI;
using PlatformerPOC.Domain;
using PlatformerPOC.Helpers;
using PlatformerPOC.Messages;

namespace PlatformerPOC.Events
{
    public class StartGameHandler : IListener<StartGameMessage>
    {
        private readonly PlatformGame _game;
        
        private List<string> AvailableNames = new List<string>();

        public StartGameHandler(PlatformGame game)
        {
            _game = game;
        }

        public void Handle(StartGameMessage message)
        {
            AvailableNames = _game.GameDataLoader.GetBotNames().Shuffle().ToList();

            _game.gameWorld.BuildWorld(_game.GameDataLoader.GetDemoLevel());

            _game.RoundCounter = 1;

            if (_game.GameMode is EliminationGameMode)
            {
                _game.gameWorld.LocalPlayer = new Player(_game, "Player 1", _game.GameDataLoader.CharacterSheets.ElementAt(2));
                _game.gameWorld.LocalPlayer.SwitchTeam(Team.Neutral);
                _game.gameWorld.Players.Add(_game.gameWorld.LocalPlayer);

                for (int i = 2; i < 4; i++)
                {
                    var botPlayer = new Player(_game, string.Format("{0} [Bot]", GetRandomName()), _game.GameDataLoader.CharacterSheets.First());
                    botPlayer.SwitchTeam(Team.Neutral);
                    botPlayer.AI = new DummyAIController();
                    _game.gameWorld.Players.Add(botPlayer);
                }
            }
            else
            {
                _game.gameWorld.LocalPlayer = new Player(_game, "Player 1", _game.GameDataLoader.CharacterSheets.First());
                _game.gameWorld.LocalPlayer.SwitchTeam(Team.Red);
                _game.gameWorld.Players.Add(_game.gameWorld.LocalPlayer);

                for (int i = 2; i < 9; i++)
                {
                    var botPlayer = new Player(_game, string.Format("{0} [Bot]", GetRandomName()), _game.GameDataLoader.CharacterSheets.First());
                    botPlayer.SwitchTeam(Team.Red);
                    botPlayer.AI = new DummyAIController();
                    _game.gameWorld.Players.Add(botPlayer);
                }
                for (int i = 9; i < 17; i++)
                {
                    var botPlayer = new Player(_game, string.Format("{0} [Bot]", GetRandomName()), _game.GameDataLoader.CharacterSheets.First());
                    botPlayer.SwitchTeam(Team.Blue);
                    botPlayer.AI = new DummyAIController();
                    _game.gameWorld.Players.Add(botPlayer);
                }
            }

            PlatformGame.eventAggregationManager.SendMessage(new SpawnPlayersMessage());
        }

        private string GetRandomName()
        {
            var item = AvailableNames.First();
            AvailableNames.RemoveAt(0);
            return item;
        }
    }
}