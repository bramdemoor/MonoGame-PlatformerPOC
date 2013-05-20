using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PlatformerPOC.Seeding
{
    public class GameData
    {
        public List<Spritesheet> Spritesheets { get; set; }
        public List<string> BotNames { get; set; }
        public List<LevelData> Levels { get; set; }
        public List<Tileset> Tilesets { get; set; }
    }

    public class Tileset
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string SourceFile { get; set; }
        public int TileSize { get; set; }
        public List<Tile> Tiles { get; set; }
    }

    public class Tile
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public TileType Type { get; set; }
    }

    // Note: This includes the objects!
    public enum TileType
    {
        Solid,
        PowerUp,
        Spawner,
        PistolBullet
    }

    public class Spritesheet
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string SourceFile { get; set; }
        public int TileSize { get; set; }
        public List<Animation> Animations { get; set; }
    }

    public class Animation
    {
        public string Name { get; set; }
        public bool LoopWithReverse { get; set; }
        public int Y { get; set; }
        public int StartX { get; set; }
        public int EndX { get; set; }        
    }

    public class LevelData
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Author { get; private set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<string> TilesetImports { get; set; }
        public List<AliasImport> TileImports { get; set; }
        public List<ParallaxLayer> Parallax { get; set; }
        public string Content { get; set; }

        // This gets filled in by our code
        public Dictionary<Vector2, LevelObjectWrapper> LevelObjects { get; set; }
        public Rectangle TilesArea { get; set; }
    }

    public class AliasImport
    {
        public string Alias { get; set; }
        public string Key { get; set; }
    }

    public class ParallaxLayer
    {
        public string SourceFile { get; set; }
        public float Speed { get; set; }
    }
}