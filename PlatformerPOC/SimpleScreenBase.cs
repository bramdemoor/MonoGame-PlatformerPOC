using Microsoft.Xna.Framework;

namespace PlatformerPOC
{
    public abstract class SimpleScreenBase
    {
        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}