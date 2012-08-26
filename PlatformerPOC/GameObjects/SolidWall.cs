using Microsoft.Xna.Framework;

namespace PlatformerPOC.GameObjects
{
    /// <summary>
    /// "Solid" object (wall, floor,...)
    /// </summary>
    public class SolidWall : PlatformGameObject
    {
        public TileDefinition TileDefinition { get; private set; }

        public Rectangle RectangleCollisionBounds 
        { 
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, TileDefinition.TileSet.TileSize.Width, TileDefinition.TileSet.TileSize.Height);
            } 
        }        

        public SolidWall(Vector2 position, TileDefinition tileDefinition)
        {
            Position = position;
            TileDefinition = tileDefinition;
        }

        public override void Draw()
        {
            TileDefinition.DrawTile(Position);            
        }
    }
}