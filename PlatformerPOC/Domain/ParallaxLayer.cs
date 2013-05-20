using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.Domain
{
    public class ParallaxLayer
    {
        public float Speed { get; set; }
        public Texture2D Texture { get; set; }

        public ParallaxLayer(float speed, Texture2D texture)
        {
            Speed = speed;
            Texture = texture;
        }
    }
}