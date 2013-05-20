using System;
using Microsoft.Xna.Framework;

namespace PlatformerPOC.Helpers
{
    /// <summary>
    /// Camera shaker. Code example: http://xnaessentials.com/archive/2011/04/26/shake-that-camera.aspx
    /// </summary>
    public class CamShaker
    {
        // TODO BDM: Clean up code

        public static bool IsShaking { get; set; }

        // The maximum magnitude of our shake offset
        private static float shakeMagnitude;

        // The total duration of the current shake
        private static float shakeDuration;

        // A timer that determines how far into our shake we are
        private static float shakeTimer;

        // The shake offset vector
        private static Vector2 shakeOffset;

        private static readonly Random random = new Random();

        /// <summary>
        /// Helper to generate a random float in the range of [-1, 1].
        /// </summary>
        private static float NextFloat()
        {
            return (float)random.NextDouble() * 2f - 1f;
        }

        public static void StartShaking(float magnitude, float duration)
        {
            CamShaker.shakeTimer = 0;       // Reset!
            CamShaker.IsShaking = true;
            CamShaker.shakeMagnitude = magnitude;
            CamShaker.shakeDuration = duration;
        }

        public static Vector2 ShakeIfShaking(Vector2 position, GameTime gameTime)
        {
            if (!IsShaking) return position;

            // Move our timer ahead based on the elapsed time
            shakeTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;

            // If we're at the max duration, we're not going to be shaking anymore
            if (shakeTimer >= shakeDuration)
            {
                IsShaking = false;
                shakeTimer = shakeDuration;
            }

            // Compute our progress in a [0, 1] range
            float progress = shakeTimer/shakeDuration;

            // Compute our magnitude based on our maximum value and our progress. This causes
            // the shake to reduce in magnitude as time moves on, giving us a smooth transition
            // back to being stationary. We use progress * progress to have a non-linear fall 
            // off of our magnitude. We could switch that with just progress if we want a linear 
            // fall off.
            float magnitude = shakeMagnitude*(1f - (progress*progress));

            // Generate a new offset vector with three random values and our magnitude
            shakeOffset = new Vector2(NextFloat(), NextFloat())*magnitude;

            // If we're shaking, add our offset to our position and target



            return position + shakeOffset;
        }
    }
}