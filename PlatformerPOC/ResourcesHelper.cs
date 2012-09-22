using GameEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.GameObjects;

namespace PlatformerPOC
{
    public class ResourcesHelper
    {
        public static CustomSpriteSheetDefinition BulletImpactSpriteSheet { get; private set; }

        public static Texture2D BgLayer1Texture { get; private set; }
        public static Texture2D BgLayer2Texture { get; private set; }        

        public static Texture2D BulletTexture { get; private set; }

        public static CustomSpriteSheetDefinition PlayerSpriteSheet { get; private set; }
        public static SoundEffect SpawnSound { get; private set; }

        public static SpriteFont DefaultFont { get; private set; }

        public static CustomTileSetDefinition MainTileSet { get; private set; }
        public static TileDefinition TileWall { get; private set; }
        public static TileDefinition TileGround { get; private set; }


        public static void LoadContent(ContentManager content)
        {
            BulletImpactSpriteSheet = new CustomSpriteSheetDefinition(content, "bullet-impact", new Rectangle(0, 0, 42, 29), 6);

            BgLayer1Texture = content.Load<Texture2D>("parallax-layer1");
            BgLayer2Texture = content.Load<Texture2D>("parallax-layer2");
            
            BulletTexture = content.Load<Texture2D>("bullet");

            PlayerSpriteSheet = new CustomSpriteSheetDefinition(content, "player", new Rectangle(0, 0, 32, 32), 8);
            SpawnSound = content.Load<SoundEffect>("testsound");

            DefaultFont = content.Load<SpriteFont>("spriteFont1");            

            MainTileSet = new CustomTileSetDefinition(content, "tileset", new Rectangle(0, 0, 32, 32));
            TileWall = new TileDefinition(MainTileSet, 0, 1);
            TileGround = new TileDefinition(MainTileSet, 0, 3);
        }
    }
}