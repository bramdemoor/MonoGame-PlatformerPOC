using Microsoft.Xna.Framework;
using PlatformerPOC.Drawing;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Domain
{
    /// <summary>
    /// "Solid" object (wall, floor,...)
    /// </summary>
    public class StaticTile : BaseGameObject
    {
        public TileDefinition TileDefinition { get; private set; }

        public StaticTile(Vector2 position, TileDefinition tileDefinition)
        {
            Position = position;
            TileDefinition = tileDefinition;

            BoundingBox = new CustomBoundingBox();
            BoundingBox.SetFullRectangle(Position, TileDefinition.TileSet.TileSize, Vector2.Zero);
        }
    }
}