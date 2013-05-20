using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Drawing;
using YamlDotNet.RepresentationModel.Serialization;

namespace PlatformerPOC.Seeding
{
    public class ResourcePreloader
    {
        public const string levelResourceString = "PlatformerPOC.Content.Levels.{0}.txt";

        private readonly PlatformGame game;

        public readonly List<CustomSpriteSheetDefinition> CharacterSheets = new List<CustomSpriteSheetDefinition>();

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
            using(var sr = new StreamReader(content.RootDirectory + @"\Characters\Spritesheets.yml"))
            {
                foreach (var sheet in new YamlSerializer<SpriteSheetData>().Deserialize(sr).Spritesheets)
                {
                    var src = ("Characters/" + sheet.SourceFile);
                    var rect = new Rectangle(0, 0, sheet.TileSize, sheet.TileSize);
                    var length = (sheet.Animations.First().EndX - sheet.Animations.First().StartX + 1);
                    CharacterSheets.Add(new CustomSpriteSheetDefinition(content, src, rect, length));    
                }
            }
                        
            // Weapons
            Pistol = content.Load<Texture2D>("Weapons/pistol");
            BulletImpactSpriteSheet = new CustomSpriteSheetDefinition(content, "Weapons/bullet-impact", new Rectangle(0, 0, 42, 29), 6);
            BulletTexture = content.Load<Texture2D>("Weapons/bullet");

            // Level
            BgLayer1Texture = content.Load<Texture2D>("Levels/parallax-layer1");
            BgLayer2Texture = content.Load<Texture2D>("Levels/parallax-layer2");
            MainTileSet = new CustomTileSetDefinition(content, "Levels/forest-tileset", new Rectangle(0, 0, 32, 32));
            TileWall = new TileDefinition(MainTileSet, 0, 1);
            TileGround = new TileDefinition(MainTileSet, 0, 3);

            // Hud
            HudText = content.Load<Texture2D>("Hud/hud");
            HudMsg = content.Load<Texture2D>("Hud/hud-msg");

            // Sounds
            SpawnSound = content.Load<SoundEffect>("Sound/testsound");

            // Fonts
            DefaultFont = content.Load<SpriteFont>("Fonts/spriteFont1");

            ObjectTiles = new CustomTileSetDefinition(content, "icon0.png", new Rectangle(0,0,16,16));

            pixel = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White }); // so that we can draw whatever color we want on top of it 
        }

        public LevelData LoadLevelData(string levelFilename)
        {
            var level = new LevelData();

            var rowIndex = 0;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format(ResourcePreloader.levelResourceString, levelFilename)))
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    if (rowIndex == 0)
                    {
                        // Meta-line
                        var parts = reader.ReadLine().Split('|');
                        level.SetMetaInformation(parts.ElementAt(0), parts.ElementAt(1), parts.ElementAt(2), parts.ElementAt(3));
                    }
                    else
                    {
                        var line = reader.ReadLine();
                        var chars = line.ToCharArray();

                        for (var colIndex = 0; colIndex < chars.Length; colIndex++)
                        {
                            level.SetTile(colIndex, rowIndex - 1, chars.ElementAt(colIndex));
                        }
                    }

                    rowIndex++;
                }
            }

            return level;
        } 
    }
}