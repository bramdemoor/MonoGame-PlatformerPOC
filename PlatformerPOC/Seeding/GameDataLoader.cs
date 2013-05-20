using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Drawing;
using YamlDotNet.RepresentationModel.Serialization;

namespace PlatformerPOC.Seeding
{
    public class GameDataLoader
    {
        private readonly PlatformGame game;

        public readonly List<CustomSpriteSheetDefinition> CharacterSheets = new List<CustomSpriteSheetDefinition>();

        private readonly Dictionary<string, Texture2D> DynamicTextures = new Dictionary<string, Texture2D>();

        public Texture2D HudText { get; private set; }
        public Texture2D HudMsg { get; private set; }        
        public SoundEffect SpawnSound { get; private set; }
        public SpriteFont DefaultFont { get; private set; }

        public Texture2D pixelForLines;
        

        private GameData gameData;

        public GameDataLoader(PlatformGame game)
        {
            this.game = game;         
        }

        public void LoadContent(ContentManager content)
        {
            pixelForLines = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixelForLines.SetData(new[] { Color.White });
            HudText = content.Load<Texture2D>("hud");
            HudMsg = content.Load<Texture2D>("hud-msg");
            SpawnSound = content.Load<SoundEffect>("Sound/testsound");
            DefaultFont = content.Load<SpriteFont>("spriteFont1");

            using(var sr = new StreamReader(content.RootDirectory + @"\bzDat.yml"))
            {
                gameData = new YamlSerializer<GameData>().Deserialize(sr);
            }

            foreach (var sheet in gameData.Spritesheets)
            {
                var rect = new Rectangle(0, 0, sheet.TileSize, sheet.TileSize);
                var length = (sheet.Animations.First().EndX - sheet.Animations.First().StartX + 1);
                CharacterSheets.Add(new CustomSpriteSheetDefinition(content, sheet.SourceFile, rect, length));
            }

            foreach (var bgToLoad in gameData.Levels.SelectMany(l => l.Parallax).Select(p => p.SourceFile).Distinct())
            {
                DynamicTextures.Add(bgToLoad, content.Load<Texture2D>(bgToLoad));
            }   
         
            foreach (var tilesetToLoad in gameData.Tilesets.Select(t => t.SourceFile).Distinct())
            {
                DynamicTextures.Add(tilesetToLoad, content.Load<Texture2D>(tilesetToLoad));
            }
                        
            //TileWall = new TileDefinition(MainTileSet, 0, 1);
            //TileGround = new TileDefinition(MainTileSet, 0, 3);

            // TODO BDM: Fix
            //BulletImpactSpriteSheet = new CustomSpriteSheetDefinition(content, "Weapons/bullet-impact", new Rectangle(0, 0, 42, 29), 6);            
        }

        public List<string> GetBotNames()
        {
            return gameData.BotNames;
        }

        public LevelData GetDemoLevel()
        {
            var demoLevel = gameData.Levels.First();

            demoLevel.LevelObjects = new Dictionary<Vector2, string>();

            var ls = demoLevel.Content.Split('\n');

            demoLevel.TilesArea = new Rectangle(0,0,ls.Max(l => l.Length), ls.Count());

            var lineIndex = 0;
            foreach(var line in ls)
            {
                var chars = line.ToCharArray();
                for(int i = 0; i<chars.Length; i++)
                {
                    if(chars[i] != ' ')
                    {
                        demoLevel.LevelObjects.Add(new Vector2(i, lineIndex), chars[i].ToString());
                    }
                }
                lineIndex++;
            }

            return demoLevel;
        }

        public Texture2D GetTextureByKey(string bgKey)
        {
            return DynamicTextures[bgKey];
        }
    }
}