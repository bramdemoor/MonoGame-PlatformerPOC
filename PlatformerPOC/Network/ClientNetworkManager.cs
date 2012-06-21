using Lidgren.Network;
using log4net;

namespace PlatformerPOC.Network
{
    public class ClientNetworkManager : INetworkManager
    {
        private static NetClient s_client;

        private ILog log;

        private bool isDisposed;

        public void Connect()
        {
            log = LogManager.GetLogger(typeof(ClientNetworkManager));

            log.Info("Initializing client...");

            //netClient = new NetClient(NetPeerConfigurationFactory.CreateForClient());

            var config = new NetPeerConfiguration(Config.networkName);

            s_client = new NetClient(config);

            s_client.Start();

            NetOutgoingMessage hail = s_client.CreateMessage();

            hail.Write("This is the hail message");

            var connection = s_client.Connect(Config.defaultIp, Config.port, hail);

            if (connection.Status != NetConnectionStatus.Connected)
            {
                log.Info("Failed to connect.");
            }
            else
            {
                log.Info("Connected!");
            }  
        }

        public void Disconnect()
        {
            s_client.Disconnect("Requested by user");
            s_client.Shutdown("Requested by user");
        }

        public void ReadMessage()
        {
            NetIncomingMessage im;
            while ((im = s_client.ReadMessage()) != null)
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
                        var status = (NetConnectionStatus)im.ReadByte();

                        string reason = im.ReadString();
                        log.Info(status.ToString() + ": " + reason);

                        break;
                    case NetIncomingMessageType.Data:
                        string chat = im.ReadString();
                        log.Info(chat);
                        break;
                    default:
                        log.Info("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
                        break;
                }
            }
        }

        public void Recycle(NetIncomingMessage im)
        {
            s_client.Recycle(im);
        }

        public NetOutgoingMessage CreateMessage()
        {
            return s_client.CreateMessage();
        }

        //public void SendMessage(IGameMessage gameMessage)
        //{
        //    NetOutgoingMessage om = netClient.CreateMessage();
        //    om.Write((byte)gameMessage.MessageType);
        //    gameMessage.Encode(om);

        //    netClient.SendMessage(om, NetDeliveryMethod.ReliableUnordered);
        //}

        public void Send(string text)
        {
            NetOutgoingMessage om = s_client.CreateMessage(text);
            s_client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
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
    }
}