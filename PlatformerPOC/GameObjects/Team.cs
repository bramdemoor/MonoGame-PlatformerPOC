using Microsoft.Xna.Framework;

namespace PlatformerPOC.GameObjects
{
    public enum PlayerTeams
    {
        NotSet = -1,
        Red = 1,
        Blue = 2
    }

    public class Team
    {
        public PlayerTeams TeamType { get; set; }

        public Team(PlayerTeams teamType)
        {
            TeamType = teamType;
        }

        public Color TeamColor
        {
            get
            {
                switch (TeamType)
                {
                    case PlayerTeams.Red:
                        return Color.Red;
                    case PlayerTeams.Blue:
                        return Color.Blue;
                    default:
                        return Color.White;
                }
            }
        }

        public Color TeamColorDark
        {
            get
            {
                switch (TeamType)
                {
                    case PlayerTeams.Red:
                        return Color.DarkRed;
                    case PlayerTeams.Blue:
                        return Color.DarkBlue;
                    default:
                        return Color.FromNonPremultiplied(30, 30, 30, 255);
                }
            }
        }

        public string TeamName
        {
            get
            {
                switch (TeamType)
                {
                    case PlayerTeams.Red:
                        return "Red team";
                    case PlayerTeams.Blue:
                        return "Blue team";
                    default:
                        return "No team";
                }                
            }
        }
    }
}