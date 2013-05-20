namespace PlatformerPOC.Helpers
{
    public class TeamEliminationGameMode : GameMode
    {
        public override string Name
        {
            get { return "Team Elimination"; }
        }

        public override string Description
        {
            get { return "Be the last surviving team!"; }
        }

        public override bool AreTeamsEnabled
        {
            get { return true; }
        }
    }
}