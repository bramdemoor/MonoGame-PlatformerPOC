using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        // TODO BDM: Make spawnpoints objects!
        // Hardcoded spawnpoint locations
        private readonly List<Vector2> spawnPointTiles = new List<Vector2> { new Vector2(3, 10), new Vector2(15, 10) };

        public Level(PlatformGame game)
        {

            this.game = game;

            LoadDemoLevel();

            game.ViewPort.LevelArea = new Rectangle(0,0, 660, 450);
        }

        private void LoadDemoLevel()
        {
            // Test for viewing all embedded resources:
            // var auxList = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();


            int rowIndex = 0;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PlatformerPOC.Content.level.txt"))
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var chars = line.ToCharArray();

                    for (int colIndex = 0; colIndex < chars.Length; colIndex++)
                    {
                        var c = chars.ElementAt(colIndex);

                        if (c == 'G')
                        {
                            AddTile(colIndex, rowIndex, game.ResourcesHelper.TileGround);
                        }
                        if (c == 'x')
                        {
                            AddTile(colIndex, rowIndex, game.ResourcesHelper.TileWall);
                        }
                    }

                    rowIndex++;
                }
            }
        }

        private void AddTile(int x, int y, TileDefinition tileDefinition)
        {
            game.AddObject(new SolidWall(game, LevelTileConcept.TilesToPixels(x, y), tileDefinition));
        }

        public void Draw()
        {
            var viewPort = game.ViewPort;

            var layer1Pos = new Vector2(-viewPort.ViewPos.X*PARALLAX_LAYER1_SPEED, -viewPort.ViewPos.Y * PARALLAX_LAYER1_SPEED);
            var layer2Pos = new Vector2(-viewPort.ViewPos.X*PARALLAX_LAYER2_SPEED, -viewPort.ViewPos.Y * PARALLAX_LAYER2_SPEED);

            game.SpriteBatch.Draw(game.ResourcesHelper.BgLayer1Texture, layer1Pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, LayerDepths.BG_PARALLAX_1);
            game.SpriteBatch.Draw(game.ResourcesHelper.BgLayer2Texture, layer2Pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, LayerDepths.BG_PARALLAX_2);
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

        public bool IsPlaceFreeOfWalls(Rectangle collisionRectangle)
        {
            return game.GameObjects.OfType<SolidWall>().All(wall => !CollisionHelper.RectangleCollision(collisionRectangle, wall.BoundingBox.FullRectangle));
        }
    }
}