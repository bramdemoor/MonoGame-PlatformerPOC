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
            // TODO BDM: Remove hardcoded values

            // Test!
            ViewArea = new Rectangle(20, 20, 600, 400);
        }

        public void DrawDebug()
        {
            var rectangleToDraw = new Rectangle(0, 0, ViewArea.Width, ViewArea.Height);
           
            SimpleGameEngine.Instance.Game.DebugDrawHelper.DrawBorder(SimpleGameEngine.Instance.spriteBatch, rectangleToDraw, 5, Color.Yellow);
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
            return new Vector2(realWorldCoords.X - ViewArea.X, realWorldCoords.Y - ViewArea.Y);
        }

        /// <summary>
        /// Converts world rectangle into view rectangle
        /// </summary>        
        public Rectangle GetRelativeCoords(Rectangle realWorldRectangle)
        {
            return new Rectangle(realWorldRectangle.X - ViewArea.X, realWorldRectangle.Y - ViewArea.Y, realWorldRectangle.Width, realWorldRectangle.Height);
        }

        public void ScrollTo(Vector2 position)
        {
            var potentialX = (int) position.X - (ViewArea.Width/2);

            var potentialY = (int) position.Y - (ViewArea.Height/2);

            // TODO BDM: Checks on 'level out of bounds'

            ViewArea = new Rectangle(potentialX, potentialY, ViewArea.Width, ViewArea.Height);
        }
    }
}