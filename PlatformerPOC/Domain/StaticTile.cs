using Microsoft.Xna.Framework;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Domain
{
    /// <summary>
    /// "Solid" object (wall, floor,...)
    /// </summary>
    public class StaticTile : BaseGameObject
    {
        public string TileKey { get; set; }

        public StaticTile(Vector2 position, string tileKey)
        {
            TileKey = tileKey;

            Position = position;

            BoundingBox = new CustomBoundingBox();
            BoundingBox.SetFullRectangle(Position, new Rectangle(0, 0, 32, 32), Vector2.Zero);
        }
    }
}