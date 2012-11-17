using System.Collections.Generic;
using GameEngine.Spritesheet;
using GameEngine.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.GameObjects;
using PlatformerPOC.Level;

namespace PlatformerPOC
{
    public class ResourcesHelper
    {
        private readonly PlatformGame game;

        public CustomSpriteSheetDefinition BulletImpactSpriteSheet { get; private set; }

        public Texture2D BgLayer1Texture { get; private set; }
        public Texture2D BgLayer2Texture { get; private set; }
        public Texture2D HudText { get; private set; }
        public Texture2D HudMsg { get; private set; }     
   
        public List<Character> Characters { get; set; }

        public Texture2D Pistol { get; private set; }        

        public Texture2D BulletTexture { get; private set; }
        
        public SoundEffect SpawnSound { get; private set; }

        public SpriteFont DefaultFont { get; private set; }

        public CustomTileSetDefinition MainTileSet { get; private set; }
        public TileDefinition TileWall { get; private set; }
        public TileDefinition TileGround { get; private set; }

        public ResourcesHelper(PlatformGame game)
        {
            this.game = game;

            Characters = new List<Character>();            
        }

        public void LoadContent(ContentManager content)
        {
            BulletImpactSpriteSheet = new CustomSpriteSheetDefinition(content, "bullet-impact", new Rectangle(0, 0, 42, 29), 6);

            BgLayer1Texture = content.Load<Texture2D>("parallax-layer1");
            BgLayer2Texture = content.Load<Texture2D>("parallax-layer2");
            HudText = content.Load<Texture2D>("hud");
            HudMsg = content.Load<Texture2D>("hud-msg");

            Characters.Add(Character.Create(content, CharacterKeys.Blue));
            Characters.Add(Character.Create(content, CharacterKeys.Pink));
            Characters.Add(Character.Create(content, CharacterKeys.Yellow));            

            Pistol = content.Load<Texture2D>("pistol");
            
            BulletTexture = content.Load<Texture2D>("bullet");

            
            
            SpawnSound = content.Load<SoundEffect>("testsound");

            DefaultFont = content.Load<SpriteFont>("spriteFont1");

            MainTileSet = new CustomTileSetDefinition(content, "tileset", new Rectangle(0, 0, 32, 32));
            TileWall = new TileDefinition(game, MainTileSet, 0, 1);
            TileGround = new TileDefinition(game, MainTileSet, 0, 3);
        }
    }
}