using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class CustomBoundingBox
    {

        public Rectangle FullRectangle { get; private set; }

        public Rectangle BottomRectangle { get; private set; }
        public Rectangle RightRectangle { get; private set; }
        public Rectangle LeftRectangle { get; private set; }
        public Rectangle TopRectangle { get; private set; }

        public void SetFullRectangle(Rectangle rectangle, Vector2 velocity)
        {
            FullRectangle = rectangle;
            TopRectangle = new Rectangle(FullRectangle.X, (int)(FullRectangle.Top + velocity.Y), FullRectangle.Width, 1);
            BottomRectangle = new Rectangle(FullRectangle.X, (int)(FullRectangle.Bottom + velocity.Y), FullRectangle.Width, 1);
            LeftRectangle = new Rectangle((int)(FullRectangle.X + velocity.X), FullRectangle.Y, 1, FullRectangle.Height);
            RightRectangle = new Rectangle((int)(FullRectangle.Right + velocity.X), FullRectangle.Y, 1, FullRectangle.Height);
        }
    }
}