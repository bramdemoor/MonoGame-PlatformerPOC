﻿using System.Collections.Generic;
using System.Linq;
using PlatformerPOC.Control;
using PlatformerPOC.Domain;
using PlatformerPOC.Domain.Gamemodes;
using PlatformerPOC.Domain.Teams;
using PlatformerPOC.Events;
using PlatformerPOC.Helpers;
using PlatformerPOC.Messages;

namespace PlatformerPOC.Handlers
{
    public class StartGameHandler : IListener<StartGameMessage>
    {
        private readonly PlatformGame _game;

        // TODO BDM: Move to config!
        private readonly string[] aiNames = new[] { "Seth", "Aryss", "Zenith", "Athena", "Necroth", "Sphinx", "Imhotep", "Avalanche", "Hydra", "Brutus", "Thor", "Harbinger", "Medusa", "Bulldog", "Vengeance", "Viper", "Wyvern", "Ghoul", "Incisor" };

        private List<string> AvailableNames = new List<string>();

        public StartGameHandler(PlatformGame game)
        {
            _game = game;
        }

        public void Handle(StartGameMessage message)
        {
            AvailableNames = aiNames.Shuffle().ToList();

            _game.RoundCounter = 1;
            _game.LevelManager.StartLevel();
   
            if (_game.GameMode is EliminationGameMode)
            {
                _game.LocalPlayer = new Player(_game, "Player 1", _game.ResourcePreloader.Character1Sheet);
                _game.LocalPlayer.SwitchTeam(Team.Neutral);
                _game.Players.Add(_game.LocalPlayer);

                for (int i = 2; i < 4; i++)
                {
                    var botPlayer = new Player(_game, string.Format("{0} [Bot]", GetRandomName()), _game.ResourcePreloader.Character2Sheet);
                    botPlayer.SwitchTeam(Team.Neutral);
                    botPlayer.AI = new DummyAIController();
                    _game.Players.Add(botPlayer);
                }
            }
            else
            {
                _game.LocalPlayer = new Player(_game, "Player 1", _game.ResourcePreloader.Character1Sheet);
                _game.LocalPlayer.SwitchTeam(Team.Red);
                _game.Players.Add(_game.LocalPlayer);

                for (int i = 2; i < 9; i++)
                {
                    var botPlayer = new Player(_game, string.Format("{0} [Bot]", GetRandomName()), _game.ResourcePreloader.Character2Sheet);
                    botPlayer.SwitchTeam(Team.Red);
                    botPlayer.AI = new DummyAIController();
                    _game.Players.Add(botPlayer);
                }
                for (int i = 9; i < 17; i++)
                {
                    var botPlayer = new Player(_game, string.Format("{0} [Bot]", GetRandomName()), _game.ResourcePreloader.Character2Sheet);
                    botPlayer.SwitchTeam(Team.Blue);
                    botPlayer.AI = new DummyAIController();
                    _game.Players.Add(botPlayer);
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