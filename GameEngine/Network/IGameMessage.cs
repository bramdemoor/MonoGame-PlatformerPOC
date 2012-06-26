using Lidgren.Network;

namespace GameEngine.Network
{
    public enum GameMessageTypes
    {
        UpdatePosition
    }

    public interface IGameMessage
    {
        GameMessageTypes MessageType { get; }

        void Decode(NetIncomingMessage im);

        void Encode(NetOutgoingMessage om);
    }
}