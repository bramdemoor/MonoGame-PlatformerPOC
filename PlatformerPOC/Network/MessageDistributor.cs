using System;
using GameEngine.Network;
using Lidgren.Network;
using PlatformerPOC.Network.Messages;

namespace PlatformerPOC.Network
{
    public class MessageDistributor : IMessageDistributor
    {
        public PlatformGame Game { get; set; }

        public MessageDistributor(PlatformGame game)
        {
            Game = game;
        }

        public void Handle(NetIncomingMessage im)
        {
            var gameMessageType = (GameMessageTypes)im.ReadByte();
                        
            switch (gameMessageType)
            {
                case GameMessageTypes.UpdatePosition:
                    break;
                case GameMessageTypes.UpdatePlayerState:
                    break;
                case GameMessageTypes.ServerStartGame:
                    var msg = new HostStartGameMessage();
                    msg.Decode(im);
                    Game.StartGame();
                    break;
                case GameMessageTypes.ClientJoinGame:
                    var msg2 = new JoinGameMessage();
                    msg2.Decode(im);
                    Game.PlayerManager.AddPlayer(msg2.Name);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}