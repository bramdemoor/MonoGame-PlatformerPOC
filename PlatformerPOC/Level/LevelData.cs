using Microsoft.Xna.Framework;

namespace PlatformerPOC.Level
{
    public class LevelData
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Author { get; private set; }
        public char[,] tiles;

        public Rectangle TilesArea
        {
            get
            {
                return new Rectangle(0, 0, tiles.GetUpperBound(0), tiles.GetUpperBound(1));
            }
        }

        public void SetMetaInformation(string name, string description, string size, string author)
        {
            Name = name;
            Description = description;
            Author = author;

            if (size == "1x1")
            {
                tiles = new char[33, 25];
            }
            else if (size == "2x1")
            {
                tiles = new char[65, 25];
            }
        }

        public void SetTile(int x, int y, char c)
        {
            tiles[x, y] = c;
        }
    }
}