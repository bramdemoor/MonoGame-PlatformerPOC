using Microsoft.Xna.Framework;

namespace GameEngine
{
    public abstract class SimpleScreenBase
    {
        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}