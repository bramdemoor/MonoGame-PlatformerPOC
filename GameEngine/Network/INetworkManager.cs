using System;

namespace GameEngine.Network
{
    public interface INetworkManager : IDisposable
    {
        bool IsConnected { get; }
        void Connect();
        void Disconnect();
        void ReadMessages();
        void Send(IGameMessage message);        
    }
}
