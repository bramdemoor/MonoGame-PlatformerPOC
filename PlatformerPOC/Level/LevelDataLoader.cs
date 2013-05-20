using System.IO;
using System.Linq;
using System.Reflection;

namespace PlatformerPOC.Level
{
    public class LevelDataLoader
    {
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