using Lidgren.Network;

namespace PlatformerPOC.Network
{
    public static class NetPeerConfigurationFactory
    {
         public static NetPeerConfiguration CreateForServer()
         {
             var configuration = CreateBasic();

             configuration.Port = Config.port;

             return configuration;
         }

        public static NetPeerConfiguration CreateForClient()
        {
            var configuration = CreateBasic();

            // TODO BDM: Results in socket conflict!
            //configuration.Port = Config.port;

            return configuration;
        }

        private static NetPeerConfiguration CreateBasic()
         {
             var config = new NetPeerConfiguration(Config.networkName)
             {
                 SimulatedMinimumLatency = Config.SimulatedMinimumLatency,
                 SimulatedLoss = Config.SimulatedLoss
             };

             config.EnableMessageType(NetIncomingMessageType.WarningMessage);
             config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
             config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
             config.EnableMessageType(NetIncomingMessageType.Error);
             config.EnableMessageType(NetIncomingMessageType.DebugMessage);
             config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

             return config;
         }
    }
}