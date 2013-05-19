using Lidgren.Network;
using Microsoft.Xna.Framework;
using PlatformerPOC.GameObjects;

namespace PlatformerPOC.Network.Messages
{
    public class UpdatePlayerStateMessage : IGameMessage
    {
        public long Id { get; set; }
        public double MessageTime { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.UpdatePlayerState;
            }
        }

        public void Decode(NetIncomingMessage im)
        {
            Id = im.ReadInt64();
            MessageTime = im.ReadDouble();            
            Position = im.ReadVector2();
            Velocity = im.ReadVector2();
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(Id);
            om.Write(MessageTime);
            om.Write(Position);
            om.Write(Velocity);
        }

        public UpdatePlayerStateMessage(NetIncomingMessage im)
        {
            Decode(im);
        }

        public UpdatePlayerStateMessage(Player player)
        {
            Position = player.Position;
            Velocity = player.Velocity;
            MessageTime = NetTime.Now;
        }
    }
}