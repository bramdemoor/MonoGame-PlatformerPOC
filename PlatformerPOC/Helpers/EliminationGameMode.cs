namespace PlatformerPOC.Helpers
{
    public class EliminationGameMode : GameMode
    {
        public override string Name
        {
            get { return "Elimination"; }
        }

        public override string Description
        {
            get { return "Be the last surviving player!"; }
        }

        public override bool AreTeamsEnabled
        {
            get { return false; }
        }
    }
}