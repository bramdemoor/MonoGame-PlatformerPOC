using System;
using System.Collections.Generic;
using Lidgren.Network;
using log4net;

namespace GameEngine.Network
{
    public class ServerNetworkManager : INetworkManager
    {
        private static NetServer netServer;

        private ILog log;

        private bool isDisposed;

        public bool IsConnected
        {
            get
            {
                return netServer != null && netServer.Status == NetPeerStatus.Running;
            }
        }

        public void Send(IGameMessage message)
        {
            var outgoing = netServer.CreateMessage();

            message.Encode(outgoing);

            if (CoreConfig.VerboseDebugOutput)
            {
                log.InfoFormat("Sending: {0}", message);
            }

            netServer.SendToAll(outgoing, NetDeliveryMethod.ReliableOrdered);            
        }

        public void Dispose()
        {
            if (isDisposed) return;

            if (true)
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
                log.Info("Server started! Listening on port " + CoreConfig.Port);
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
                        log.Info(im.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        var status = (NetConnectionStatus)im.ReadByte();

                        switch (status)
                        {
                            case NetConnectionStatus.None:
                                break;
                            case NetConnectionStatus.InitiatedConnect:
                                break;
                            case NetConnectionStatus.RespondedConnect:
                                break;
                            case NetConnectionStatus.Connected:
                                break;
                            case NetConnectionStatus.Disconnecting:

                                var uid2 = im.SenderConnection.Peer.UniqueIdentifier;
                                log.Info("DISCONNECTING !!! => " + uid2);

                                break;
                            case NetConnectionStatus.Disconnected:
                                // TODO BDM: Handle Disconnect

                                var uid = im.SenderConnection.Peer.UniqueIdentifier;
                                log.Info("DISCONNECTED!!! => " + uid);

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        string reason = im.ReadString();
                        log.Info(string.Format("{0} {1}: {2}", NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier), status, reason));

                        UpdateConnectionsList();
                        break;
                    case NetIncomingMessageType.Data:
                        string chat = im.ReadString();

                        log.Info("Broadcasting '" + chat + "'");
                        
                        var all = netServer.Connections; // broadcast this to all connections, except sender (get copy and remove)
                        all.Remove(im.SenderConnection);

                        if (all.Count > 0)
                        {
                            var om = netServer.CreateMessage();
                            om.Write(string.Format("{0} said: {1}", NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier), chat));
                            netServer.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                        }
                        break;
                    default:
                        log.Info(string.Format("Unhandled type: {0} {1} bytes {2}|{3}", im.MessageType, im.LengthBytes, im.DeliveryMethod, im.SequenceChannel));
                        break;
                }

            }
        }

        private void UpdateConnectionsList()
        {
            foreach (var conn in netServer.Connections)
            {
                string str = string.Format("{0} from {1} [{2}]", NetUtility.ToHexString(conn.RemoteUniqueIdentifier), conn.RemoteEndpoint.ToString(), conn.Status);
                log.Info(str);
            }
        }
    }
}