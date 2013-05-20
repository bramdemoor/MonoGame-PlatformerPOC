﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Drawing;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Domain
{
    /// <summary>
    /// Base Game object    
    /// </summary>public abstract void Draw();
    public abstract class BaseGameObject
    {
        protected readonly PlatformGame game;

        public long Id { get; set; }

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


        public ViewPort ViewPort
        {
            get { return game.ViewPort; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return game.SpriteBatch; }
        }

        public bool InView
        {
            get
            {
                if (BoundingBox == null) return true;   // Otherwise, objects without bounding box can never be drawn!
                return ViewPort.IsObjectInArea(BoundingBox.FullRectangle);
            }
        }

        public Vector2 PositionRelativeToView
        {
            get { return ViewPort.GetRelativeCoords(Position); }
        }       

        public BaseGameObject(PlatformGame game)
        {
            this.game = game;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public void DestroyEntity()
        {
            game.DeleteObject(this);
        }


        public virtual void DrawDebug()
        {

        }

        public void PlaySound(SoundEffect spawnSound)
        {
            if (!Config.SoundEnabled) return;

            SoundEffectInstance soundEffectInstance = spawnSound.CreateInstance();

            soundEffectInstance.Volume = Config.SoundVolume;

            soundEffectInstance.Play();
        }

        public abstract void Draw();
    }
}