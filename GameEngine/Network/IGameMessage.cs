using Lidgren.Network;

namespace GameEngine.Network
{
    // TODO BDM: Move out of core engine!

    public enum GameMessageTypes
    {
        UpdatePosition,
        UpdatePlayerState,
        ServerStartGame,
        ClientJoinGame
    }

    public interface IGameMessage
    {
        GameMessageTypes MessageType { get; }

        void Decode(NetIncomingMessage im);

        void Encode(NetOutgoingMessage om);
    }
}