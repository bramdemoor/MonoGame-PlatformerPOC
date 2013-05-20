namespace PlatformerPOC.Helpers
{
    public interface IPlayerControlState
    {
        bool IsMoveLeftPressed { get; }

        bool IsMoveRightPressed { get; }

        bool IsMoveDownPressed { get; }

        bool IsMoveUpPressed { get; }

        bool IsActionPressed { get; }
    }
}