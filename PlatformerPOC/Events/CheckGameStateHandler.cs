﻿using System.Linq;
using PlatformerPOC.Messages;

namespace PlatformerPOC.Events
{
    public class CheckGameStateHandler: IListener<CheckGameStateMessage>
    {
        private readonly PlatformGame _game;

        public CheckGameStateHandler(PlatformGame game)
        {
            _game = game;
        }

        public void Handle(CheckGameStateMessage message)
        {
            switch (_game.gameWorld.AlivePlayers.Count())
            {
                case 0:
                    PlatformGame.eventAggregationManager.SendMessage(new SpawnPlayersMessage());

                    _game.RoundCounter++;
                    break;
                case 1:
                    // Only 1 player? Don't do the checks.
                    if (_game.gameWorld.Players.Count() == 1) return;

                    var winner = _game.gameWorld.AlivePlayers.Single();
                    winner.Score.MarkWin();
                    PlatformGame.eventAggregationManager.SendMessage(new SpawnPlayersMessage());

                    _game.RoundCounter++;

                    break;
                default:
                    // continue game
                    break;
            }
        }
    }
}