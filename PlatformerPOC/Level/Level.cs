using System.Collections.Generic;
using System.Linq;
using GameEngine;
using GameEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.GameObjects
{
    public class Level
    {
        private const float PARALLAX_LAYER1_SPEED = 0.6f;
        private const float PARALLAX_LAYER2_SPEED = 0.9f;

        // TODO BDM: Make spawnpoints objects!
        // Hardcoded spawnpoint locations
        private readonly List<Vector2> spawnPointTiles = new List<Vector2> { new Vector2(3, 10), new Vector2(15, 10) };

        public Level()
        {
            BuildHardcodedLevel();
        }

        private void BuildHardcodedLevel()
        {
            for (int y = 0; y < 14; y++)
            {
                // Left border
                AddTile(0, y, ResourcesHelper.TileWall);
            }        

            for (int y = 0; y < 14; y++)
            {
                // Right border
                AddTile(19, y, ResourcesHelper.TileWall);                
            }   
     
            for (int x = 0; x < 20; x++)
            {
                // Floor
                AddTile(x, 14, ResourcesHelper.TileGround);
            }

            AddTile(7, 12, ResourcesHelper.TileWall);
            AddTile(8, 12, ResourcesHelper.TileWall);
            AddTile(9, 12, ResourcesHelper.TileWall);

            AddTile(8, 10, ResourcesHelper.TileWall);
            AddTile(6, 8, ResourcesHelper.TileWall);
        }

        private static void AddTile(int x, int y, TileDefinition tileDefinition)
        {
            PlatformGame.Instance.AddObject(new SolidWall(LevelTileConcept.TilesToPixels(x, y), tileDefinition));
        }

        public void Draw()
        {
            var viewPort = PlatformGame.Instance.ViewPort;

            var layer1Pos = new Vector2(-viewPort.ViewPos.X*PARALLAX_LAYER1_SPEED, -viewPort.ViewPos.Y * PARALLAX_LAYER1_SPEED);
            var layer2Pos = new Vector2(-viewPort.ViewPos.X*PARALLAX_LAYER2_SPEED, -viewPort.ViewPos.Y * PARALLAX_LAYER2_SPEED);

            SimpleGameEngine.Instance.spriteBatch.Draw(ResourcesHelper.BgLayer1Texture, layer1Pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, LayerDepths.BG_PARALLAX_1);
            SimpleGameEngine.Instance.spriteBatch.Draw(ResourcesHelper.BgLayer2Texture, layer2Pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, LayerDepths.BG_PARALLAX_2);
        }

        public bool IsInBoundsLeft(Vector2 position)
        {
            return position.X > LevelTileConcept.TilesToPixels(0);
        }

        public bool IsInBoundsRight(Vector2 position)
        {
            return position.X < LevelTileConcept.TilesToPixels(19);
        }

        public bool IsInBounds(Vector2 position)
        {
            return IsInBoundsLeft(position) && IsInBoundsRight(position);
        }

        public Vector2 GetNextFreeSpawnPoint()
        {
            return LevelTileConcept.TilesToPixels(spawnPointTiles.First());
        }

        public bool Get(Vector2 position)
        {
            foreach (var wall in PlatformGame.Instance.GameObjects.OfType<SolidWall>())
            {
                if (CollisionHelper.PointCollision(position, wall.BoundingBox.FullRectangle))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsPlaceFree(Rectangle collisionRectangle)
        {
            foreach (var wall in PlatformGame.Instance.GameObjects.OfType<SolidWall>())
            {
                if(CollisionHelper.RectangleCollision(collisionRectangle, wall.BoundingBox.FullRectangle))
                {
                    return false;
                }
            }

            return true;
        }
    }
}