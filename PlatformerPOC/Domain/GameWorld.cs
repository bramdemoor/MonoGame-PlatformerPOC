﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.Control;
using PlatformerPOC.Helpers;
using PlatformerPOC.Seeding;

namespace PlatformerPOC.Domain
{
    /// <summary>
    /// "Physical" game world
    /// </summary>
    public class GameWorld
    {
        public const int SquareSize = 32;

        private readonly PlatformGame game;

        public List<Player> Players { get; set; }
        public List<Projectile> Bullets { get; set; }
        public Player LocalPlayer { get; set; }
        public List<StaticTile> Walls { get; set; }
        public List<Powerup> Coins { get; set; }
        public readonly List<Vector2> spawnPointPositions = new List<Vector2>();
        public readonly List<ParallaxLayer> bgLayers = new List<ParallaxLayer>();

        public LevelData CurrentLevelData { get; private set; }

        private bool IsInBoundsLeft(Vector2 position)
        {
            return position.X > TilesToPixels(0);
        }

        private bool IsInBoundsRight(Vector2 position)
        {
            return position.X < TilesToPixels(19);
        }

        public bool IsInBounds(Vector2 position)
        {
            return IsInBoundsLeft(position) && IsInBoundsRight(position);
        }

        public IEnumerable<Player> AlivePlayers
        {
            get { return Players.Where(p => p.IsAlive); }
        }

        public GameWorld(PlatformGame game)
        {
            this.game = game;
            Players = new List<Player>();
            Bullets = new List<Projectile>();
            Walls = new List<StaticTile>();
            Coins = new List<Powerup>();
        }

        public void BuildWorld(LevelData data)
        {
            CurrentLevelData = data;

            for (int x = 0; x < data.tiles.GetUpperBound(0); x++)
            {
                for (int y = 0; y < data.tiles.GetUpperBound(1); y++)
                {
                    var levelPos = TilesToPixels(x, y);
                    var c = data.tiles[x, y];

                    if (c == 'G')
                    {
                        Walls.Add(new StaticTile(levelPos, game.ResourcePreloader.TileGround));
                    }
                    else if (c == 'x')
                    {
                        Walls.Add(new StaticTile(levelPos, game.ResourcePreloader.TileWall));
                    }
                    else if (c == 'S')
                    {
                        spawnPointPositions.Add(levelPos);
                    }
                    else if (c == '*')
                    {
                        Coins.Add(new Powerup(levelPos));
                    }
                }
            }

            bgLayers.Add(new ParallaxLayer(0.6f, game.ResourcePreloader.BgLayer1Texture));
            bgLayers.Add(new ParallaxLayer(0.9f, game.ResourcePreloader.BgLayer2Texture));
            game.renderer.LevelArea = TilesToPixels(data.TilesArea);
        }

        public void Update(GameTime gameTime)
        {
            LocalPlayer.HandleInput(new PlayerKeyboardState(Keyboard.GetState()));

            foreach (var player in Players)
            {
                if (player.AI != null)
                {
                    player.AI.Evaluate(player.Position, LocalPlayer.Position, PlatformGame.Randomizer);
                    player.HandleInput(player.AI);
                }

                player.Update(gameTime);
            }

            foreach (var bullet in Bullets)
            {
                bullet.Update(gameTime);
            }
        }

        public bool IsPlaceFreeOfWalls(Rectangle collisionRectangle)
        {
            return Walls.All(wall => !CollisionHelper.RectangleCollision(collisionRectangle, wall.BoundingBox.FullRectangle));
        }
        
        private static Vector2 TilesToPixels(int tileX, int tileY)
        {
            return new Vector2(tileX * SquareSize, tileY * SquareSize);
        }

        private static int TilesToPixels(int tiles)
        {
            return tiles * SquareSize;
        }

        public static Vector2 TilesToPixels(Vector2 tiles)
        {
            return tiles * SquareSize;
        }

        public static Vector2 PixelsToTiles(Vector2 worldCoords)
        {
            return new Vector2((float)Math.Floor(worldCoords.X / SquareSize), (float)Math.Floor(worldCoords.Y / SquareSize));
        }

        private static Rectangle TilesToPixels(Rectangle tilesRectangle)
        {
            return new Rectangle(TilesToPixels(tilesRectangle.X), TilesToPixels(tilesRectangle.Y), TilesToPixels(tilesRectangle.Width), TilesToPixels(tilesRectangle.Height));
        }
    }
}