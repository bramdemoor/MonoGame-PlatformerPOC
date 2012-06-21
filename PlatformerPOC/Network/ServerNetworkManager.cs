using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Lidgren.Network;
using log4net;

namespace PlatformerPOC.Network
{
    public class ServerNetworkManager : INetworkManager
    {
        private static NetServer netServer;

        private ILog log;

        private bool isDisposed;

        //public void SendMessage(IGameMessage gameMessage)
        //{
        //    NetOutgoingMessage om = netServer.CreateMessage();
        //    om.Write((byte)gameMessage.MessageType);
        //    gameMessage.Encode(om);

        //    netServer.SendToAll(om, NetDeliveryMethod.ReliableUnordered);
        //}

        public void Send(string text)
        {
            NetOutgoingMessage om = netServer.CreateMessage(text);
            netServer.SendToAll(om, NetDeliveryMethod.ReliableOrdered);            
        }

        public bool IsConnected
        {
            get
            {
                return netServer != null 
                    && netServer.Status == NetPeerStatus.Running;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                Disconnect();
            }

            isDisposed = true;
        }

        public void Connect()
        {
            log = LogManager.GetLogger(typeof(ServerNetworkManager));

            log.Info("Initializing server...");

            netServer = new NetServer(NetPeerConfigurationFactory.CreateForServer());

            netServer.Start();

            if (netServer.Status == NetPeerStatus.Running)
            {
                log.Info("Server started! Listening on port " + Config.Port);
            }
            else
            {
                log.Error("Failed to start server!");
            }
        }

        public void Disconnect()
        {
            log.Info("Stopping server...");

            netServer.Shutdown("Requested by user");

            if (netServer.Status == NetPeerStatus.ShutdownRequested)
            {
                log.Info("Server stopped!");
            }
            else
            {
                log.Info("Failed to stop server");
            }
        }

        public void ReadMessages()
        {
            NetIncomingMessage im;
            while ((im = netServer.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                        string text = im.ReadString();
                        log.Info(text);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                        string reason = im.ReadString();
                        log.Info(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);

                        UpdateConnectionsList();
                        break;
                    case NetIncomingMessageType.Data:
                        // incoming chat message from a client
                        string chat = im.ReadString();

                        log.Info("Broadcasting '" + chat + "'");

                        // broadcast this to all connections, except sender
                        List<NetConnection> all = netServer.Connections; // get copy
                        all.Remove(im.SenderConnection);

                        if (all.Count > 0)
                        {
                            NetOutgoingMessage om = netServer.CreateMessage();
                            om.Write(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " said: " +
                                     chat);
                            netServer.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                        }
                        break;
                    default:
                        log.Info("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel);
                        break;
                }

            }
        }

        private void UpdateConnectionsList()
        {
            foreach (NetConnection conn in netServer.Connections)
            {
                string str = NetUtility.ToHexString(conn.RemoteUniqueIdentifier) + " from " + conn.RemoteEndpoint.ToString() + " [" + conn.Status + "]";
                log.Info(str);
            }
        }

        public void Recycle(NetIncomingMessage im)
        {
            netServer.Recycle(im);
        }

        public NetOutgoingMessage CreateMessage()
        {
            return netServer.CreateMessage();
        }
    }
}