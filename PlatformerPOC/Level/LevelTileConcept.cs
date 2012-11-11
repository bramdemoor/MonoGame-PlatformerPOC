using Microsoft.Xna.Framework;

namespace PlatformerPOC.Level
{
    public static class LevelTileConcept
    {
        public const int SquareSize = 32;

        public static Vector2 TilesToPixels(int tileX, int tileY)
        {
            return new Vector2(tileX * SquareSize, tileY * SquareSize);
        }

        public static int TilesToPixels(int tiles)
        {
            return tiles * SquareSize;
        }

        public static Vector2 TilesToPixels(Vector2 tiles)
        {
            return tiles * SquareSize;
        } 
    }
}