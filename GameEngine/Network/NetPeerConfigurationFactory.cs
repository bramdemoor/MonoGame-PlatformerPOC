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
            return CreateBasic();
        }

        private static NetPeerConfiguration CreateBasic()
         {
            return new NetPeerConfiguration(CoreConfig.NetworkName);
         }
    }
}