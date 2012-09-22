using GameEngine;
using Microsoft.Xna.Framework;

namespace PlatformerPOC.GameObjects
{
    /// <summary>
    /// "Solid" object (wall, floor,...)
    /// </summary>
    public class SolidWall : PlatformGameObject
    {
        public TileDefinition TileDefinition { get; private set; }

        public SolidWall(Vector2 position, TileDefinition tileDefinition)
        {
            Position = position;
            TileDefinition = tileDefinition;

            BoundingBox = new CustomBoundingBox();
            BoundingBox.SetFullRectangle(Position, TileDefinition.TileSet.TileSize, Vector2.Zero);
        }

        public override void Draw()
        {
            if (ViewPort.IsObjectInArea(BoundingBox.FullRectangle))
            {
                TileDefinition.DrawTile(ViewPort.GetRelativeCoords(Position), LayerDepths.TILES);
            }            
        }

        public override void DrawDebug()
        {
            var rel = ViewPort.GetRelativeCoords(BoundingBox.FullRectangle);

            PlatformGame.Instance.DebugDrawHelper.DrawBorder(SpriteBatch, rel, 2, Color.DarkRed);
        }
    }
}