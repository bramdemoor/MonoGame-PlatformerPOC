using Microsoft.Xna.Framework;

namespace GameEngine
{
    /// <summary>
    /// Simple 'scollable' viewport implementation
    /// </summary>
    public class ViewPort
    {
        public Rectangle ViewArea { get; set; }

        public ViewPort()
        {
            // Test!
            ViewArea = new Rectangle(20, 20, 600, 600);
        }

        /// <summary>
        /// Simple check for objects
        /// </summary>
        public bool IsObjectInArea(Rectangle objectRectangle)
        {
            return ViewArea.Intersects(objectRectangle);
        }

        /// <summary>
        /// Converts world coordinates into view coordinates
        /// </summary>        
        public Vector2 GetRelativeCoords(Vector2 realWorldCoords)
        {
            return new Vector2(realWorldCoords.X + ViewArea.X, realWorldCoords.Y + ViewArea.Y);
        }
    }
}