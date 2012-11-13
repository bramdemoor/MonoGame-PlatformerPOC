using GameEngine.Spritesheet;
using GameEngine.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC
{
    public class ResourcesHelper
    {
        private readonly PlatformGame game;

        public CustomSpriteSheetDefinition BulletImpactSpriteSheet { get; private set; }

        public Texture2D BgLayer1Texture { get; private set; }
        public Texture2D BgLayer2Texture { get; private set; }
        public Texture2D HudText { get; private set; }        

        public Texture2D BulletTexture { get; private set; }

        public CustomSpriteSheetDefinition PlayerSpriteSheet { get; private set; }
        public SoundEffect SpawnSound { get; private set; }

        public SpriteFont DefaultFont { get; private set; }

        public CustomTileSetDefinition MainTileSet { get; private set; }
        public TileDefinition TileWall { get; private set; }
        public TileDefinition TileGround { get; private set; }

        public ResourcesHelper(PlatformGame game)
        {
            this.game = game;
        }

        public void LoadContent(ContentManager content)
        {
            BulletImpactSpriteSheet = new CustomSpriteSheetDefinition(content, "bullet-impact", new Rectangle(0, 0, 42, 29), 6);

            BgLayer1Texture = content.Load<Texture2D>("parallax-layer1");
            BgLayer2Texture = content.Load<Texture2D>("parallax-layer2");
            HudText = content.Load<Texture2D>("hud");
            
            BulletTexture = content.Load<Texture2D>("bullet");

            PlayerSpriteSheet = new CustomSpriteSheetDefinition(content, "player", new Rectangle(0, 0, 32, 32), 8);
            SpawnSound = content.Load<SoundEffect>("testsound");

            DefaultFont = content.Load<SpriteFont>("spriteFont1");            

            MainTileSet = new CustomTileSetDefinition(content, "tileset", new Rectangle(0, 0, 32, 32));
            TileWall = new TileDefinition(game, MainTileSet, 0, 1);
            TileGround = new TileDefinition(game, MainTileSet, 0, 3);
        }
    }
}