using System;
using Lidgren.Network;

namespace GameEngine.Network
{
    public interface INetworkManager : IDisposable
    {
        void Connect();
        void Disconnect();

        void ReadMessages();

        void Recycle(NetIncomingMessage im);

        NetOutgoingMessage CreateMessage();

        void Send(string text);
        bool IsConnected { get; }
    }
}
