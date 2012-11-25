using GameEngine.Network;
using Lidgren.Network;

namespace PlatformerPOC.Network.Messages
{
    public class JoinGameMessage : IGameMessage
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public GameMessageTypes MessageType
        {
            get { return GameMessageTypes.ClientJoinGame;}
        }

        public JoinGameMessage()
        {
        }

        public JoinGameMessage(string name)
        {
            Name = name;
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write((int)MessageType);
            om.Write(Id);
            om.Write(Name);
        }

        public void Decode(NetIncomingMessage im)
        {
            Id = im.ReadInt64();
            Name = im.ReadString();
        }
    }
}