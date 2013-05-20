using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using PlatformerPOC.Domain.Powerup;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Domain.Level
{
    public class Level
    {
        private readonly PlatformGame game;

        public readonly List<BgLayer> bgLayers = new List<BgLayer>();
        
        private readonly List<Vector2> spawnPointPositions = new List<Vector2>();

        public List<SolidWall> Walls { get; set; }
        public List<Coin> Coins { get; set; }

        private char[,] tiles;

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Author { get; private set; }

        public Rectangle TilesArea
        {
            get
            {
                return new Rectangle(0, 0, tiles.GetUpperBound(0), tiles.GetUpperBound(1));
            }
        }

        public Level(PlatformGame game)
        {
            this.game = game;

            Walls = new List<SolidWall>();
            Coins = new List<Coin>();
        }

        public void SetMetaInformation(string name, string description, string size, string author)
        {
            Name = name;
            Description = description;
            Author = author;
            
            if(size == "1x1")
            {
                tiles = new char[33, 25];
            }
            else if (size == "2x1")
            {
                tiles = new char[65, 25];
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
            return Walls.All(wall => !CollisionHelper.RectangleCollision(collisionRectangle, wall.BoundingBox.FullRectangle));
        }

        public void Clear()
        {
            spawnPointPositions.Clear();
        }

        public void CompleteLoad()
        {
            for (int x = 0; x < tiles.GetUpperBound(0); x++)
            {
                for (int y = 0; y < tiles.GetUpperBound(1); y++)
                {
                    var levelPos = LevelTileConcept.TilesToPixels(x, y);
                    var c = tiles[x, y];

                    if (c == 'G')
                    {
                        Walls.Add(new SolidWall(game, levelPos, game.ResourcePreloader.TileGround));                        
                    }
                    else if (c == 'x')
                    {
                        Walls.Add(new SolidWall(game, levelPos, game.ResourcePreloader.TileWall));                        
                    }
                    else if (c == 'S')
                    {
                        spawnPointPositions.Add(levelPos);
                    }
                    else if(c == '*')
                    {
                        Coins.Add(new Coin(game, levelPos));                        
                    }
                }
            }
        }

        public void SetBgLayers()
        {
            bgLayers.Add(new BgLayer(game, LayerType.First, game.ResourcePreloader.BgLayer1Texture));
            bgLayers.Add(new BgLayer(game, LayerType.Second, game.ResourcePreloader.BgLayer2Texture));
        }

        public void SetTile(int x, int y, char c)
        {
            tiles[x, y] = c;
        }
    }
}