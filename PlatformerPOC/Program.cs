namespace PlatformerPOC
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var platformGame = new PlatformGame();

            platformGame.Run();
            platformGame.StartGame(); 
        } 
    }
}