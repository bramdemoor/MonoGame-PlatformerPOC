using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace PlatformerPOC.Network
{
    /// <summary>
    /// Allows writing and reading XNA-specific value types over the network stream
    /// Based on XNAExtensions in Lidgren XNA Extensions
    /// </summary>
    public static class XNAExtensions
    {
        /// <summary>
        /// Writes a Vector2
        /// </summary>
        public static void Write(this NetOutgoingMessage message, Vector2 vector)
        {
            message.Write(vector.X);
            message.Write(vector.Y);
        }

        /// <summary>
        /// Reads a Vector2
        /// </summary>
        public static Vector2 ReadVector2(this NetIncomingMessage message)
        {
            Vector2 retval;
            retval.X = message.ReadSingle();
            retval.Y = message.ReadSingle();
            return retval;
        }
    }
}