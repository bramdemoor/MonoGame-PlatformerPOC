using Lidgren.Network;

namespace GameEngine.Network
{
    public interface IMessageDistributor
    {
        void Handle(NetIncomingMessage im);
    }
}