using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Helpers;
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
                //TODO BDM: Clean up very brittle and hacky code!!!
                var anim = sheet.Animations.First();
                var rect = new Rectangle(0, 0, sheet.TileSize, sheet.TileSize);
                var length = (sheet.Animations.First().EndX - sheet.Animations.First().StartX + 1);
                var def = new CustomSpriteSheetDefinition(content, sheet.SourceFile, rect, length);
                def.Y = anim.Y;
                CharacterSheets.Add(def);
            }

            foreach (var bgToLoad in gameData.Levels.SelectMany(l => l.Parallax).Select(p => p.SourceFile).Distinct())
            {
                DynamicTextures.Add(bgToLoad, content.Load<Texture2D>(bgToLoad));
            }   
         
            foreach (var tilesetToLoad in gameData.Tilesets.Select(t => t.SourceFile).Distinct())
            {
                DynamicTextures.Add(tilesetToLoad, content.Load<Texture2D>(tilesetToLoad));
            }
                        
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

            demoLevel.LevelObjects = new Dictionary<Vector2, LevelObjectWrapper>();

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
                        var tileAlias = chars[i].ToString();

                        var tileMapping = demoLevel.TileImports.FirstOrDefault(imp => imp.Alias == tileAlias);

                        if (tileMapping != null)
                        {
                            var importedTileSets = gameData.Tilesets.Where(t => demoLevel.TilesetImports.Contains(t.Name)).ToArray();

                            if (!importedTileSets.Any())
                                throw new ArgumentException("Please import at least 1 tileset!");

                            var tileDefinition = importedTileSets.SelectMany(t => t.Tiles).FirstOrDefault(tile => tile.Name == tileMapping.Key);

                            if (tileDefinition == null)
                                throw new ArgumentException("Tile definition not found: " + tileMapping.Key);

                            demoLevel.LevelObjects.Add(new Vector2(i, lineIndex), new LevelObjectWrapper(tileMapping.Key, tileDefinition.Type));
                        }
                        else
                        {
                            // Ignore for now
                            //throw new ArgumentException("Tile alias not found: " + tileAlias);
                        }
                    }
                }
                lineIndex++;
            }

            return demoLevel;
        }

        public Texture2D GetTextureByKey(string textureKey)
        {
            return DynamicTextures[textureKey];
        }

        public Texture2D GetTextureByTileKey(string tileKey)
        {
            var set = gameData.Tilesets.FirstOrDefault(s => s.Tiles.Any(t => t.Name == tileKey));
            if(set != null)
            {
                return GetTextureByKey(set.SourceFile);
            }
            else
            {
                throw new ArgumentException("Tileset/Texture not found for tile key: " + tileKey);
            }            
        }

        public Rectangle? GetRectangleByTileKey(string tileKey)
        {
            var set = gameData.Tilesets.FirstOrDefault(s => s.Tiles.Any(t => t.Name == tileKey));
            if (set != null)
            {
                var def = set.Tiles.FirstOrDefault(t => t.Name == tileKey);
                if(def != null)
                {
                    return new Rectangle(def.X * set.TileSize, def.Y * set.TileSize, set.TileSize, set.TileSize);
                }                
            }

            throw new ArgumentException("Tileset coords not found for tile key: " + tileKey);
        }
    }

    public class LevelObjectWrapper
    {
        public string Key { get; set; }
        public TileType Type { get; set; }

        public LevelObjectWrapper(string key, TileType type)
        {
            Key = key;
            Type = type;
        }
    }
}