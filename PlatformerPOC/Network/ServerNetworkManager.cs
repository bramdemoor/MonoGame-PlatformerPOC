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
        private static NetServer s_server;

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
            NetOutgoingMessage om = s_server.CreateMessage(text);
            s_server.SendMessage(om, s_server.Connections, NetDeliveryMethod.ReliableOrdered, 1);
            //s_server.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            //log.Info("Sending '" + text + "'");
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

            //NetPeerConfigurationFactory.CreateForServer()

            var config = new NetPeerConfiguration(Config.networkName);
            config.MaximumConnections = Config.MaximumConnections;
            config.Port = Config.port;
            s_server = new NetServer(config);

            s_server.Start();

            if (s_server.Status == NetPeerStatus.Running)
            {
                log.Info("Server started! Listening on port " + Config.port);
            }
            else
            {
                log.Error("Failed to start server!");
            }
        }

        public void Disconnect()
        {
            log.Info("Stopping server...");

            s_server.Shutdown("Requested by user");

            if (s_server.Status == NetPeerStatus.ShutdownRequested)
            {
                log.Info("Server stopped!");
            }
            else
            {
                log.Info("Failed to stop server");
            }
        }

        public void ReadMessage()
        {
            NetIncomingMessage im;
            while ((im = s_server.ReadMessage()) != null)
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
                        List<NetConnection> all = s_server.Connections; // get copy
                        all.Remove(im.SenderConnection);

                        if (all.Count > 0)
                        {
                            NetOutgoingMessage om = s_server.CreateMessage();
                            om.Write(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " said: " +
                                     chat);
                            s_server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
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
            foreach (NetConnection conn in s_server.Connections)
            {
                string str = NetUtility.ToHexString(conn.RemoteUniqueIdentifier) + " from " + conn.RemoteEndpoint.ToString() + " [" + conn.Status + "]";
                log.Info(str);
            }
        }

        public void Recycle(NetIncomingMessage im)
        {
            s_server.Recycle(im);
        }

        public NetOutgoingMessage CreateMessage()
        {
            return s_server.CreateMessage();
        }
    }
}