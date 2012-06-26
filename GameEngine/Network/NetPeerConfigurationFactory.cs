using Lidgren.Network;

namespace GameEngine.Network
{
    public static class NetPeerConfigurationFactory
    {
         public static NetPeerConfiguration CreateForServer()
         {
             var configuration = CreateBasic();

             configuration.Port = CoreConfig.Port;
             configuration.MaximumConnections = CoreConfig.MaximumConnections;

             return configuration;
         }

        public static NetPeerConfiguration CreateForClient()
        {
            var configuration = CreateBasic();

            return configuration;
        }

        private static NetPeerConfiguration CreateBasic()
         {
             var config = new NetPeerConfiguration(CoreConfig.NetworkName)
             {
                 SimulatedMinimumLatency = CoreConfig.SimulatedMinimumLatency,
                 SimulatedLoss = CoreConfig.SimulatedLoss
             };

             return config;
         }
    }
}