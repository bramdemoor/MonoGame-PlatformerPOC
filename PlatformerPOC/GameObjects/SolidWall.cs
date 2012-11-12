using GameEngine;
using GameEngine.Helpers;
using Microsoft.Xna.Framework;

namespace PlatformerPOC.GameObjects
{
    /// <summary>
    /// "Solid" object (wall, floor,...)
    /// </summary>
    public class SolidWall : PlatformGameObject
    {
        public TileDefinition TileDefinition { get; private set; }

        public SolidWall(PlatformGame game, Vector2 position, TileDefinition tileDefinition) : base(game)
        {
            Position = position;
            TileDefinition = tileDefinition;

            BoundingBox = new CustomBoundingBox();
            BoundingBox.SetFullRectangle(Position, TileDefinition.TileSet.TileSize, Vector2.Zero);
        }

        public override void Draw()
        {
            if(!InView) return;

            TileDefinition.DrawTile(PositionRelativeToView, LayerDepths.TILES);                      
        }

        public override void DrawDebug()
        {
            if (!InView) return;

            var rel = ViewPort.GetRelativeCoords(BoundingBox.FullRectangle);

            game.DebugDrawHelper.DrawBorder(rel, 2, Color.DarkRed);
        }
    }
}