using Microsoft.Xna.Framework.Input;

namespace PlatformerPOC.Domain
{
    public class PlayerKeyboardState
    {
        private KeyboardState _keyboardState;

        private const Keys moveLeftKey = Keys.Left;
        private const Keys moveRightKey = Keys.Right;

        public bool IsMoveLeftPressed
        {
            get { return _keyboardState.IsKeyDown(moveLeftKey); }
        }

        public bool IsMoveRightPressed
        {
            get { return _keyboardState.IsKeyDown(moveRightKey); }
        }

        public PlayerKeyboardState(KeyboardState keyboardState)
        {
            _keyboardState = keyboardState;
        }
    }
}