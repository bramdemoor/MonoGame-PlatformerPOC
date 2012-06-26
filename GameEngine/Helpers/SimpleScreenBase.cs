using Microsoft.Xna.Framework;

namespace GameEngine.Helpers
{
    public abstract class SimpleScreenBase
    {
        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}