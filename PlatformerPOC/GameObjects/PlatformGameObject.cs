﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Drawing;

namespace PlatformerPOC.GameObjects
{
    /// <summary>
    /// Defines some common behaviour for our platform game objects
    /// </summary>
    public abstract class PlatformGameObject : BaseGameObject
    {
        protected readonly PlatformGame game;

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

        protected PlatformGameObject(PlatformGame game) : base(game)
        {
            this.game = game;
        }

        public ViewPort ViewPort
        {
            get { return game.ViewPort; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return game.SpriteBatch; }
        }

        public override bool InView
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

        public override void DrawDebug()
        {

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