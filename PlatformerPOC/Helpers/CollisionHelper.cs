using Microsoft.Xna.Framework;

namespace PlatformerPOC.Helpers
{
    public static class CollisionHelper
    {
        public static bool RectangleCollision(Rectangle r1, Rectangle r2)
        {
            return r1.Intersects(r2);
        }
    }
}