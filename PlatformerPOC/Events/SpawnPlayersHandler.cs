using PlatformerPOC.Messages;

namespace PlatformerPOC.Events
{
    public class SpawnPlayersHandler : IListener<SpawnPlayersMessage>
    {
        private readonly PlatformGame _game;

        public SpawnPlayersHandler(PlatformGame game)
        {
            _game = game;
        }

        public void Handle(SpawnPlayersMessage message)
        {
            for (int playerIndex = 0; playerIndex < _game.Players.Count; playerIndex++)
            {
                var player = _game.Players[playerIndex];
                player.Spawn(_game.LevelManager.CurrentLevel.GetSpawnPointForPlayerIndex(playerIndex + 1));
            }
        }
    }
}