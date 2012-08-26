using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.GameObjects
{
    public class TileDefinition
    {
        private readonly CustomTileSetDefinition tileSet;

        private readonly Rectangle graphicsRectangle;

        public TileDefinition(CustomTileSetDefinition tileSet, int x, int y)
        {
            this.tileSet = tileSet;

            graphicsRectangle = tileSet.GetGraphicsRectangle(x, y);
        }


        public void DrawTile(Vector2 pos)
        {
            SimpleGameEngine.Instance.spriteBatch.Draw(tileSet.TilesetTexture, pos, graphicsRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}