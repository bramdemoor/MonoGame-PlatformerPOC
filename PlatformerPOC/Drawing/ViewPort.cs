using Microsoft.Xna.Framework;

namespace PlatformerPOC.Drawing
{
    /// <summary>
    /// Simple 'scollable' viewport implementation
    /// </summary>
    public class ViewPort
    {
        private readonly SimpleGame game;

        public Rectangle ViewArea { get; set; }

        public Rectangle LevelArea { get; set; }

        public Vector2 ViewPos
        {
            get
            {
                return new Vector2(ViewArea.X, ViewArea.Y);
            }
        }

        public ViewPort(SimpleGame game)
        {
            this.game = game;

            // TODO BDM: Move magic number out of here (doesn't belong in engine!)

            ViewArea = new Rectangle(0, 0, CoreConfig.ScreenResolutionWidth - 280, CoreConfig.ScreenResolutionHeight);
        }

        public void DrawDebug()
        {            
            var rectangleToDraw = new Rectangle(0, 0, ViewArea.Width, ViewArea.Height);

             game.DebugDrawHelper.DrawBorder(rectangleToDraw, 5, Color.Yellow);
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
        /// Converts view coordinates into world coordinates
        /// </summary>        
        public Vector2 GetWorldCoords(Vector2 viewCoords)
        {
            return new Vector2(viewCoords.X + ViewArea.X, viewCoords.Y + ViewArea.Y);
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

            if (potentialX < LevelArea.X) potentialX = LevelArea.X;
            if (potentialX > LevelArea.Right - ViewArea.Width) potentialX = LevelArea.Right - ViewArea.Width;

            if (potentialY < LevelArea.Y) potentialY = LevelArea.Y;
            if (potentialY > LevelArea.Bottom - ViewArea.Height) potentialY = LevelArea.Bottom - ViewArea.Height;

            // TODO BDM: Checks on 'level out of bounds'

            ViewArea = new Rectangle(potentialX, potentialY, ViewArea.Width, ViewArea.Height);
        }
    }
}