using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PlatformerPOC.Level
{
    public class LevelManager
    {
        private readonly PlatformGame game;

        private readonly List<Level> allLevels = new List<Level>();

        public Level CurrentLevel { get; private set; }

        public LevelManager(PlatformGame game)
        {
            this.game = game;
        }

        public void PreloadLevels()
        {
            allLevels.Clear();
            foreach (var levelFilename in game.ResourcesHelper.GetAllLevelFilenames())
            {
                allLevels.Add(PreLoadLevel(levelFilename));
            }
        }

        private Level PreLoadLevel(string levelFilename)
        {
            // FYI: Test for viewing all embedded resources:
            // var auxList = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();

            var level = new Level(game);

            level.Clear();

            var rowIndex = 0;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format(ResourcesHelper.levelResourceString, levelFilename)))
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    if (rowIndex == 0)
                    {
                        // Meta-line
                        var parts = reader.ReadLine().Split('|');
                        level.SetMetaInformation(parts.ElementAt(0), parts.ElementAt(1), parts.ElementAt(2), parts.ElementAt(3));

                        // TODO BDM: Make bg-layers dynamic too!
                        level.SetBgLayers();

                    }
                    else
                    {
                        var line = reader.ReadLine();
                        var chars = line.ToCharArray();

                        for (var colIndex = 0; colIndex < chars.Length; colIndex++)
                        {
                            level.SetTile(colIndex, rowIndex, chars.ElementAt(colIndex));
                        }
                    }

                    rowIndex++;
                }
            }
            
            game.ViewPort.LevelArea = LevelTileConcept.TilesToPixels(level.TilesArea);

            return level;
        }

        public void StartLevel()
        {
            CurrentLevel = allLevels.First();
            CurrentLevel.CompleteLoad();
        }
    }
}