using Lidgren.Network;

namespace PlatformerPOC.Network.Messages
{
    public class UpdatePositionMessage : IGameMessage
    {
        public GameMessageTypes MessageType
        {
            get { return GameMessageTypes.UpdatePosition; }
        }

        public void Decode(NetIncomingMessage im)
        {
            //this.Id = im.ReadInt64();
            //this.MessageTime = im.ReadDouble();
            //this.Position = im.ReadVector2();
            //this.Velocity = im.ReadVector2();
            //this.Rotation = im.ReadSingle();
        }

        public void Encode(NetOutgoingMessage om)
        {
            //om.Write(this.Id);
            //om.Write(this.MessageTime);
            //om.Write(this.Position);
            //om.Write(this.Velocity);
            //om.Write(this.Rotation);
        }

        public UpdatePositionMessage(NetIncomingMessage im)
        {
            Decode(im);
        }

        public UpdatePositionMessage()
        {
            //this.MessageTime = NetTime.Now;
        }
    }
}