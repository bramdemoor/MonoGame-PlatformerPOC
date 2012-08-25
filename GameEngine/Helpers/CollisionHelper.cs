using Microsoft.Xna.Framework;

namespace GameEngine.Helpers
{
    public static class CollisionHelper
    {
        public static bool RectangleCollision(Rectangle r1, Rectangle r2)
        {
            return !(r2.Left > r1.Right || r2.Right < r1.Left || r2.Top > r1.Bottom || r2.Bottom < r1.Top);
        }
    }
}