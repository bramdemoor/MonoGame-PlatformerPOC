namespace PlatformerPOC.Control.AI
{
    /// <summary>
    /// Stupid AI that runs around randomly and fires at will.
    /// </summary>
    public class DummyAIController : IPlayerControlState
    {
        public bool IsMoveLeftPressed
        {
            get { return true; }
        }

        public bool IsMoveRightPressed
        {
            get { return false; }
        }

        public bool IsMoveDownPressed
        {
            get { return false; }
        }

        public bool IsMoveUpPressed
        {
            get { return true; }
        }

        public bool IsActionPressed
        {
            get { return false; }
        }
    }
}