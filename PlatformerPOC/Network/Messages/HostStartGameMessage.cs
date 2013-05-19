using Lidgren.Network;

namespace PlatformerPOC.Network.Messages
{
    public class HostStartGameMessage : IGameMessage
    {
        public long Id { get; set; }

        public GameMessageTypes MessageType
        {
            get { return GameMessageTypes.ServerStartGame; }
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write((int) MessageType);
            om.Write(Id);
        }

        public void Decode(NetIncomingMessage im)
        {
            Id = im.ReadInt64();
        }
    }
}