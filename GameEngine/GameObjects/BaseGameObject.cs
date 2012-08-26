using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.GameObjects
{
    /// <summary>
    /// Base Game object    
    /// </summary>
    public abstract class BaseGameObject
    {
        public long Id { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Rotation { get; set; }
        public double LastUpdateTime { get; set; }

        public GameObjectState PrevDisplayState { get; set; }
        public GameObjectState DisplayState { get; set; }
        public GameObjectState SimulationState { get; set; }

        public abstract void Draw();

        public virtual void Update(GameTime gameTime)
        {
            //var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //SimulationState.Position += SimulationState.Velocity * elapsedSeconds;

            //PrevDisplayState.Position += PrevDisplayState.Velocity * elapsedSeconds;

            //ApplySmoothing(1 / 12f);
        }

        public void DestroyEntity()
        {
            SimpleGameEngine.Instance.Game.MarkGameObjectForDelete(this);
        }

        private void ApplySmoothing(float delta)
        {
            DisplayState.Position = Vector2.Lerp(PrevDisplayState.Position, SimulationState.Position, delta);

            DisplayState.Velocity = Vector2.Lerp(PrevDisplayState.Velocity, SimulationState.Velocity, delta);

            DisplayState.Rotation = MathHelper.Lerp(PrevDisplayState.Rotation, SimulationState.Rotation, delta);

            PrevDisplayState = (GameObjectState)DisplayState.Clone();
        }
    }
}