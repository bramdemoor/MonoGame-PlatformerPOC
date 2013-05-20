using System.Collections.Generic;

namespace PlatformerPOC.Seeding
{
    public class SpriteSheetData
    {
        public List<Spritesheet> Spritesheets { get; set; }
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
}