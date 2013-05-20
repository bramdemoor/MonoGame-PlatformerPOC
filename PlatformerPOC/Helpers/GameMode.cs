namespace PlatformerPOC.Helpers
{
    /// <summary>
    /// Collection of game rules
    /// </summary>
    public abstract class GameMode
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract bool AreTeamsEnabled { get; }
    }
}