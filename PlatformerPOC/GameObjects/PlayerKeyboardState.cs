using Microsoft.Xna.Framework.Input;

namespace PlatformerPOC.GameObjects
{
    public class PlayerKeyboardState
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

        public PlayerKeyboardState(KeyboardState keyboardState)
        {
            _keyboardState = keyboardState;
        }
    }
}