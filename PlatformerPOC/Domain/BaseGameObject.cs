using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Domain
{
    /// <summary>
    /// Base Game object    
    /// </summary>
    public abstract class BaseGameObject
    {
        protected readonly PlatformGame game;

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public CustomBoundingBox BoundingBox { get; set; }

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

        public bool InView
        {
            get
            {
                if (BoundingBox == null) return true;   // Otherwise, objects without bounding box can never be drawn!
                return game.ViewPort.IsObjectInArea(BoundingBox.FullRectangle);
            }
        }

        public Vector2 PositionRelativeToView
        {
            get { return game.ViewPort.GetRelativeCoords(Position); }
        }

        protected BaseGameObject(PlatformGame game)
        {
            this.game = game;
        }

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