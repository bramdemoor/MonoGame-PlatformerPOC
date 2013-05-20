using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Domain
{
    public enum HorizontalDirection
    {
        None = 0,
        Left = -1,
        Right = 1
    }

    /// <summary>
    /// Base Game object    
    /// </summary>
    public abstract class BaseGameObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public CustomBoundingBox BoundingBox { get; set; }

        public HorizontalDirection HorizontalDirection { get; set; }

        protected void DestroyEntity()
        {
            // TODO BDM: Fix
            //game.DeleteObject(this);
        }

        protected void PlaySound(SoundEffect spawnSound)
        {
            if (!Config.SoundEnabled) return;

            SoundEffectInstance soundEffectInstance = spawnSound.CreateInstance();

            soundEffectInstance.Volume = Config.SoundVolume;

            soundEffectInstance.Play();
        }
    }
}