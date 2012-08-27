using Microsoft.Xna.Framework;

namespace GameEngine
{
    /// <summary>
    /// Simple 'scollable' viewport implementation
    /// </summary>
    public class ViewPort
    {
        public Rectangle ViewArea { get; set; }

        public Vector2 ViewPos
        {
            get
            {
                return new Vector2(ViewArea.X, ViewArea.Y);
            }
        }

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
            return ViewArea.Intersects(GetRelativeCoords(objectRectangle));
        }

        /// <summary>
        /// Converts world coordinates into view coordinates
        /// </summary>        
        public Vector2 GetRelativeCoords(Vector2 realWorldCoords)
        {
            return new Vector2(realWorldCoords.X - ViewArea.X, realWorldCoords.Y - ViewArea.Y);
        }

        /// <summary>
        /// Converts world rectangle into view rectangle
        /// </summary>        
        public Rectangle GetRelativeCoords(Rectangle realWorldRectangle)
        {
            return new Rectangle(realWorldRectangle.X + ViewArea.X, realWorldRectangle.Y + ViewArea.Y, realWorldRectangle.Width, realWorldRectangle.Height);
        }

        public void ScrollTo(Vector2 position)
        {
            ViewArea = new Rectangle((int) position.X - 250, (int) position.Y - 300, ViewArea.Width, ViewArea.Height);
        }
    }
}