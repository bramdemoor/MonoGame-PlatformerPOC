namespace PlatformerPOC.GameObjects
{
    public enum GameModeTypes
    {
        Elimination,
        Team_Elimination,
        Team_CaptureTheFlag,
    }

    /// <summary>
    /// Collection of game rules
    /// </summary>
    public class GameMode
    {
        public GameModeTypes Type { get; set; }
    }
}