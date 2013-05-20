using Microsoft.Xna.Framework.Input;

namespace PlatformerPOC.Helpers
{
    public class PlayerKeyboardState : IPlayerControlState
    {
        private KeyboardState _keyboardState;

        public bool IsMoveLeftPressed
        {
            get { return _keyboardState.IsKeyDown(Config.moveLeftKey); }
        }

        public bool IsMoveRightPressed
        {
            get { return _keyboardState.IsKeyDown(Config.moveRightKey); }
        }

        public bool IsMoveDownPressed
        {
            get { return _keyboardState.IsKeyDown(Config.moveDownKey); }
        }

        public bool IsMoveUpPressed
        {
            get { return _keyboardState.IsKeyDown(Config.moveUpKey); }
        }

        public bool IsActionPressed
        {
            get { return _keyboardState.IsKeyDown(Config.actionKey); }
        }

        public PlayerKeyboardState(KeyboardState keyboardState)
        {
            _keyboardState = keyboardState;
        }
    }
}