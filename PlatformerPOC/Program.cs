using System;

namespace PlatformerPOC
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var platformGame = new PlatformGame())
            {
                platformGame.Run();
              //  platformGame.StartGame();
            }
        }
    }
}