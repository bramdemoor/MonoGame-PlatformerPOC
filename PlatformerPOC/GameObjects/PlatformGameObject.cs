using GameEngine;
using GameEngine.GameObjects;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.GameObjects
{
    /// <summary>
    /// Defines some common behaviour for our platform game objects
    /// </summary>
    public abstract class PlatformGameObject : BaseGameObject
    {
        /// <summary>
        /// -1: left
        /// 1: right        
        /// </summary>
        public int HorizontalDirection { get; set; }

        public SpriteEffects DrawEffect
        {
            get
            {
                return HorizontalDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
        }

        public GameEngine.ViewPort ViewPort
        {
            get { return PlatformGame.Instance.ViewPort; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return SimpleGameEngine.Instance.spriteBatch; }
        }

        public void PlaySound(SoundEffect spawnSound)
        {
            if (!Config.SoundEnabled) return;

            SoundEffectInstance soundEffectInstance = spawnSound.CreateInstance();

            soundEffectInstance.Volume = Config.SoundVolume;

            soundEffectInstance.Play();
        }
    }
}