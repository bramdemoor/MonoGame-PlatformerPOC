using PlatformerPOC.Control;
using PlatformerPOC.Domain;
using PlatformerPOC.Domain.Gamemodes;
using PlatformerPOC.Domain.Teams;
using PlatformerPOC.Events;
using PlatformerPOC.Messages;

namespace PlatformerPOC.Handlers
{
    public class StartGameHandler : IListener<StartGameMessage>
    {
        private readonly PlatformGame _game;

        public StartGameHandler(PlatformGame game)
        {
            _game = game;
        }

        public void Handle(StartGameMessage message)
        {
            _game.RoundCounter = 1;
            _game.LevelManager.StartLevel();
            _game._aiHelper.Reset();

            if (_game.GameMode is EliminationGameMode)
            {
                _game.LocalPlayer = new Player(_game, "Player 1", _game.ResourcePreloader.Character1Sheet);
                _game.LocalPlayer.SwitchTeam(Team.Neutral);
                _game.Players.Add(_game.LocalPlayer);
                _game.AddObject(_game.LocalPlayer);

                for (int i = 2; i < 4; i++)
                {
                    var botPlayer = new Player(_game, string.Format("{0} [Bot]", _game._aiHelper.GetRandomName()), _game.ResourcePreloader.Character2Sheet);
                    botPlayer.SwitchTeam(Team.Neutral);
                    botPlayer.AI = new DummyAIController();
                    _game.Players.Add(botPlayer);
                    _game.AddObject(botPlayer);
                }
            }
            else
            {
                _game.LocalPlayer = new Player(_game, "Player 1", _game.ResourcePreloader.Character1Sheet);
                _game.LocalPlayer.SwitchTeam(Team.Red);
                _game.Players.Add(_game.LocalPlayer);
                _game.AddObject(_game.LocalPlayer);

                for (int i = 2; i < 9; i++)
                {
                    var botPlayer = new Player(_game, string.Format("{0} [Bot]", _game._aiHelper.GetRandomName()), _game.ResourcePreloader.Character2Sheet);
                    botPlayer.SwitchTeam(Team.Red);
                    botPlayer.AI = new DummyAIController();
                    _game.Players.Add(botPlayer);
                    _game.AddObject(botPlayer);
                }
                for (int i = 9; i < 17; i++)
                {
                    var botPlayer = new Player(_game, string.Format("{0} [Bot]", _game._aiHelper.GetRandomName()), _game.ResourcePreloader.Character2Sheet);
                    botPlayer.SwitchTeam(Team.Blue);
                    botPlayer.AI = new DummyAIController();
                    _game.Players.Add(botPlayer);
                    _game.AddObject(botPlayer);
                }
            }

            PlatformGame.eventAggregationManager.SendMessage(new SpawnPlayersMessage());
        }
    }
}