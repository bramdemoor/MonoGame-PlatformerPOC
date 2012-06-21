using Microsoft.Xna.Framework;

namespace PlatformerPOC.Screens
{
    public abstract class SimpleScreen
    {
        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}