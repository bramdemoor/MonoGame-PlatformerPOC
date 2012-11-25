using System;
using Lidgren.Network;
using log4net;

namespace GameEngine.Network
{
    public class ClientNetworkManager : INetworkManager
    {
        private static NetClient netClient;

        private ILog log;

        private bool isDisposed;

        private SimpleGame game;

        public bool IsConnected
        {
            get
            {
                return netClient != null && netClient.Status == NetPeerStatus.Running && netClient.ConnectionStatus == NetConnectionStatus.Connected;
            }
        }

        public ClientNetworkManager(SimpleGame game)
        {
            this.game = game;            
        }

        public void Connect()
        {
            log = LogManager.GetLogger(typeof(ClientNetworkManager));

            log.Info("Initializing client...");

            netClient  = new NetClient(NetPeerConfigurationFactory.CreateForClient());

            netClient.Start();

            var hail = netClient.CreateMessage();

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
                        if(CoreConfig.VerboseDebugOutput)
                        {
                            var text = im.ReadString();
                            log.Info(text);                            
                        }                        
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        var status = (NetConnectionStatus)im.ReadByte();
                        string reason = im.ReadString();
                        log.Info(string.Format("{0}: {1}", status.ToString(), reason));
                        break;
                    case NetIncomingMessageType.Data:
                        game.MessageDistributor.Handle(im);
                        break;
                    default:
                        log.Info(string.Format("Unhandled type: {0} {1} bytes", im.MessageType, im.LengthBytes));
                        break;
                }
            }
        }

        public void Send(IGameMessage message)
        {
            var outgoing = netClient.CreateMessage();

            message.Encode(outgoing);

            if (CoreConfig.VerboseDebugOutput)
            {
                log.InfoFormat("Sending: {0}", message);
            } 

            netClient.SendMessage(outgoing, NetDeliveryMethod.ReliableOrdered);                                   
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
    }
}