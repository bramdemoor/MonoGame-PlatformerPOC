using Lidgren.Network;

namespace PlatformerPOC.Network
{
    public static class NetPeerConfigurationFactory
    {
         public static NetPeerConfiguration CreateForServer()
         {
             var configuration = CreateBasic();

             configuration.Port = Config.Port;
             configuration.MaximumConnections = Config.MaximumConnections;

             return configuration;
         }

        public static NetPeerConfiguration CreateForClient()
        {
            var configuration = CreateBasic();

            return configuration;
        }

        private static NetPeerConfiguration CreateBasic()
         {
             var config = new NetPeerConfiguration(Config.NetworkName)
             {
                 SimulatedMinimumLatency = Config.SimulatedMinimumLatency,
                 SimulatedLoss = Config.SimulatedLoss
             };

             return config;
         }
    }
}