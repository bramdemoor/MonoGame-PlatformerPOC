using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Drawing;
using PlatformerPOC.Level;

namespace PlatformerPOC
{
    public class ResourcePreloader
    {
        public const string levelResourceString = "PlatformerPOC.Content.Levels.{0}.txt";

        private readonly PlatformGame game;

        public CustomSpriteSheetDefinition BulletImpactSpriteSheet { get; private set; }

        public Texture2D BgLayer1Texture { get; private set; }
        public Texture2D BgLayer2Texture { get; private set; }
        public Texture2D HudText { get; private set; }
        public Texture2D HudMsg { get; private set; }     
   
        public Texture2D Pistol { get; private set; }        

        public Texture2D BulletTexture { get; private set; }
        
        public SoundEffect SpawnSound { get; private set; }

        public SpriteFont DefaultFont { get; private set; }

        public CustomTileSetDefinition MainTileSet { get; private set; }
        
        public TileDefinition TileWall { get; private set; }
        public TileDefinition TileGround { get; private set; }

        public CustomSpriteSheetDefinition Character1Sheet { get; set; }
        public CustomSpriteSheetDefinition Character2Sheet { get; set; }

        public CustomTileSetDefinition ObjectTiles { get; private set; }

        // For rectangle drawing
        public Texture2D pixel;

        public IEnumerable<string> GetAllLevelFilenames()
        {
            yield return "miniforest";
            yield return "doubleforest";
        }

        public ResourcePreloader(PlatformGame game)
        {
            this.game = game;         
        }

        public void LoadContent(ContentManager content)
        {
            // Weapons
            Pistol = content.Load<Texture2D>("Weapons/pistol");
            BulletImpactSpriteSheet = new CustomSpriteSheetDefinition(content, "Weapons/bullet-impact", new Rectangle(0, 0, 42, 29), 6);
            BulletTexture = content.Load<Texture2D>("Weapons/bullet");

            // Level
            BgLayer1Texture = content.Load<Texture2D>("Levels/parallax-layer1");
            BgLayer2Texture = content.Load<Texture2D>("Levels/parallax-layer2");
            MainTileSet = new CustomTileSetDefinition(content, "Levels/forest-tileset", new Rectangle(0, 0, 32, 32));
            TileWall = new TileDefinition(game, MainTileSet, 0, 1);
            TileGround = new TileDefinition(game, MainTileSet, 0, 3);

            // Hud
            HudText = content.Load<Texture2D>("Hud/hud");
            HudMsg = content.Load<Texture2D>("Hud/hud-msg");

            // Sounds
            SpawnSound = content.Load<SoundEffect>("Sound/testsound");

            // Fonts
            DefaultFont = content.Load<SpriteFont>("Fonts/spriteFont1");

            Character1Sheet = new CustomSpriteSheetDefinition(content, "Characters/chara1", new Rectangle(0, 0, 32, 32), 3);
            Character2Sheet = new CustomSpriteSheetDefinition(content, "Characters/player-blue", new Rectangle(0, 0, 32, 32), 8);

            ObjectTiles = new CustomTileSetDefinition(content, "icon0.png", new Rectangle(0,0,16,16));

            pixel = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White }); // so that we can draw whatever color we want on top of it 
        }


    }
}