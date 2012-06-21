using Lidgren.Network;

namespace PlatformerPOC.Network.Messages
{
    public enum GameMessageTypes
    {
        // Nothing here yet
    }

    public interface IGameMessage
    {
        GameMessageTypes MessageType { get; }

        void Decode(NetIncomingMessage im);

        void Encode(NetOutgoingMessage om);
    }
}