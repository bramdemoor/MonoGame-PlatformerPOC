using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace PlatformerPOC.Network
{
    public interface INetworkManager : IDisposable
    {
        void Connect();
        void Disconnect();

        void ReadMessage();

        void Recycle(NetIncomingMessage im);

        NetOutgoingMessage CreateMessage();

        void Send(string text);
    }
}
