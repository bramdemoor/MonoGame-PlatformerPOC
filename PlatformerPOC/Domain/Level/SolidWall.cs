using Microsoft.Xna.Framework;
using PlatformerPOC.Drawing;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Domain.Level
{
    /// <summary>
    /// "Solid" object (wall, floor,...)
    /// </summary>
    public class SolidWall : BaseGameObject
    {
        public TileDefinition TileDefinition { get; private set; }

        public SolidWall(PlatformGame game, Vector2 position, TileDefinition tileDefinition) : base(game)
        {
            Position = position;
            TileDefinition = tileDefinition;

            BoundingBox = new CustomBoundingBox();
            BoundingBox.SetFullRectangle(Position, TileDefinition.TileSet.TileSize, Vector2.Zero);
        }
    }
}