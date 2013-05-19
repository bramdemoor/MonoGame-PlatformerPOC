using Lidgren.Network;

namespace PlatformerPOC.Network
{
    public interface IMessageDistributor
    {
        void Handle(NetIncomingMessage im);
    }
}