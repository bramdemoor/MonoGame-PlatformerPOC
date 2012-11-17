using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GameEngine;
using GameEngine.Collision;
using GameEngine.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.GameObjects;

namespace PlatformerPOC.Level
{
    public class Level
    {
        private readonly PlatformGame game;

        private const float PARALLAX_LAYER1_SPEED = 0.6f;
        private const float PARALLAX_LAYER2_SPEED = 0.9f;
        
        private readonly List<Vector2> spawnPointPositions = new List<Vector2>();

        public Level(PlatformGame game)
        {
            this.game = game;

            LoadDemoLevel();
        }

        private void LoadDemoLevel()
        {
            var maxWidth = 0;

            spawnPointPositions.Clear();

            // Test for viewing all embedded resources:
            // var auxList = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();

            var rowIndex = 0;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PlatformerPOC.Content.Levels.doubleforest.txt"))
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    if(rowIndex == 0)
                    {
                        // Meta-line
                    }
                    else
                    {
                        var line = reader.ReadLine();
                        var chars = line.ToCharArray();

                        for (var colIndex = 0; colIndex < chars.Length; colIndex++)
                        {
                            var c = chars.ElementAt(colIndex);

                            var levelPos = LevelTileConcept.TilesToPixels(colIndex, rowIndex);

                            if (c == 'G')
                            {
                                AddTile(levelPos, game.ResourcesHelper.TileGround);
                            }
                            if (c == 'x')
                            {
                                AddTile(levelPos, game.ResourcesHelper.TileWall);
                            }
                            if (c == 'S')
                            {
                                spawnPointPositions.Add(levelPos);
                            }

                            if (colIndex > maxWidth) maxWidth = colIndex;
                        }                        
                    }

                    rowIndex++;
                }
            }

            var levelDimensions = LevelTileConcept.TilesToPixels(maxWidth, rowIndex);

            game.ViewPort.LevelArea = new Rectangle(0, 0, (int) levelDimensions.X, (int) levelDimensions.Y);
        }

        private void AddTile(Vector2 pos, TileDefinition tileDefinition)
        {
            game.AddObject(new SolidWall(game, pos, tileDefinition));
        }

        public void Draw()
        {
            var viewPort = game.ViewPort;

            var layer1Pos = new Vector2(-viewPort.ViewPos.X*PARALLAX_LAYER1_SPEED, 0);
            var layer2Pos = new Vector2(-viewPort.ViewPos.X*PARALLAX_LAYER2_SPEED, 0);

            game.SpriteBatch.Draw(game.ResourcesHelper.BgLayer1Texture, layer1Pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, LayerDepths.BG_PARALLAX_1);
            game.SpriteBatch.Draw(game.ResourcesHelper.BgLayer2Texture, layer2Pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, LayerDepths.BG_PARALLAX_2);

            if(CoreConfig.DebugModeEnabled)
            {
                game.DebugDrawHelper.DrawDebugString(string.Format("L1: {0}", layer1Pos), new Vector2(640, 30));
                game.DebugDrawHelper.DrawDebugString(string.Format("L2: {0}", layer2Pos), new Vector2(640, 50));
            }
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

        public Vector2 GetSpawnPointForPlayerIndex(int i)
        {
            return spawnPointPositions.ElementAtOrDefault(i - 1);
        }

        public bool IsPlaceFreeOfWalls(Rectangle collisionRectangle)
        {
            return game.GameObjects.OfType<SolidWall>().All(wall => !CollisionHelper.RectangleCollision(collisionRectangle, wall.BoundingBox.FullRectangle));
        }
    }
}