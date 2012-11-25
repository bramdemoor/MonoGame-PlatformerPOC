using System.Collections.Generic;
using System.Linq;
using GameEngine.Helpers;

namespace PlatformerPOC.Control.AI
{
    public class AINameHelper
    {
        private readonly string[] aiNames = new []
                                       {
                                           "Seth", "Aryss", "Zenith", "Athena", "Necroth", "Sphinx", "Imhotep", "Avalanche", "Hydra", "Brutus", "Thor", "Harbinger", "Medusa", "Bulldog", "Vengeance", "Viper", "Wyvern", "Ghoul", "Incisor"
                                       };

        private List<string> AvailableNames = new List<string>();

        public AINameHelper()
        {
            Reset();
        }

        /// <summary>
        /// Allow all names to be used again
        /// </summary>
        public void Reset()
        {
            AvailableNames = aiNames.Shuffle().ToList();
        }

        public string GetRandomName()
        {
            var item = AvailableNames.First();
            AvailableNames.RemoveAt(0);
            return item;
        }
    }
}