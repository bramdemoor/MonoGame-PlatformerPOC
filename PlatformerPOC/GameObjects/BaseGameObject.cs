using Microsoft.Xna.Framework;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.GameObjects
{
    /// <summary>
    /// Base Game object    
    /// </summary>
    public abstract class BaseGameObject
    {
        private readonly PlatformGame game;

        public BaseGameObject(PlatformGame game)
        {
            this.game = game;
        }

        public long Id { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }                
        
        public CustomBoundingBox BoundingBox { get; set; }

        public abstract bool InView { get; }

        public abstract void Draw();

        public abstract void DrawDebug();

        public virtual void Update(GameTime gameTime)
        {
        }

        public void DestroyEntity()
        {
            game.DeleteObject(this);
        }
    }
}