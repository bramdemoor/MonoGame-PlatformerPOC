using Microsoft.Xna.Framework;

namespace PlatformerPOC.Domain
{
    public class RedTeam : Team
    {
        public override Color TeamColor
        {
            get { return Color.Red; }
        }

        public override Color TeamColorDark
        {
            get { return Color.DarkRed; }
        }

        public override string TeamName
        {
            get { return "Red team"; }
        }
    }

    public class BlueTeam : Team
    {
        public override Color TeamColor
        {
            get { return Color.Blue; }
        }

        public override Color TeamColorDark
        {
            get { return Color.DarkBlue; }
        }

        public override string TeamName
        {
            get { return "Blue team"; }
        }
    }

    public class NeutralTeam : Team
    {
        public override Color TeamColor
        {
            get { return Color.White; }
        }

        public override Color TeamColorDark
        {
            get
            {                
                return Color.FromNonPremultiplied(30, 30, 30, 255);
            }
        }

        public override string TeamName
        {
            get { return "Neutral team"; }
        }
    }

    public abstract class Team
    {
        public abstract Color TeamColor { get; }
        public abstract Color TeamColorDark { get; }
        public abstract string TeamName { get; } 
       
        static Team()
        {
            Red = new RedTeam();
            Blue = new BlueTeam();
            Neutral = new NeutralTeam();
        }

        protected Team()
        {
            
        }

        public static Team Red { get; private set; }
        public static Team Blue { get; private set; }
        public static Team Neutral { get; private set; }
    }
}