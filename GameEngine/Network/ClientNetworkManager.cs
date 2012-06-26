using Lidgren.Network;
using log4net;

namespace GameEngine.Network
{
    public class ClientNetworkManager : INetworkManager
    {
        private static NetClient netClient;

        private ILog log;

        private bool isDisposed;

        public void Connect()
        {
            log = LogManager.GetLogger(typeof(ClientNetworkManager));

            log.Info("Initializing client...");

            netClient  = new NetClient(NetPeerConfigurationFactory.CreateForClient());

            netClient.Start();

            NetOutgoingMessage hail = netClient.CreateMessage();

            hail.Write("This is the hail message");

            var connection = netClient.Connect(CoreConfig.DefaultIp, CoreConfig.Port, hail);

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
            netClient.Disconnect("Requested by user");
            netClient.Shutdown("Requested by user");
        }

        public void ReadMessages()
        {
            NetIncomingMessage im;
            while ((im = netClient.ReadMessage()) != null)
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
            netClient.Recycle(im);
        }

        public NetOutgoingMessage CreateMessage()
        {
            return netClient.CreateMessage();
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
            NetOutgoingMessage om = netClient.CreateMessage(text);
            netClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            //log.Info("Sending '" + text + "'");
        }

        public bool IsConnected
        {
            get
            {
                return netClient != null 
                    && netClient.Status == NetPeerStatus.Running 
                    && netClient.ConnectionStatus == NetConnectionStatus.Connected;
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
    }
}