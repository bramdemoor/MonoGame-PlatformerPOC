using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.Domain.Level
{
    public enum LayerType
    {
        First,
        Second
    }

    public class BgLayer
    {
        public const float PARALLAX_LAYER1_SPEED = 0.6f;
        public const float PARALLAX_LAYER2_SPEED = 0.9f;

        private readonly PlatformGame game;

        private readonly LayerType layer;
        public readonly Texture2D texture;

        private float Speed
        {
            get
            {
                switch (layer)
                {
                    case LayerType.First:
                        return PARALLAX_LAYER1_SPEED;
                    case LayerType.Second:
                        return PARALLAX_LAYER2_SPEED;
                    default:
                        return 1f;
                }
            }
        }

        public BgLayer(PlatformGame game, LayerType type, Texture2D texture)
        {
            this.game = game;
            layer = type;
            this.texture = texture;
        }
    }
}