using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.Level
{
    public enum LayerType
    {
        First,
        Second
    }

    public class BgLayer
    {
        private const float PARALLAX_LAYER1_SPEED = 0.6f;
        private const float PARALLAX_LAYER2_SPEED = 0.9f;

        private readonly PlatformGame game;

        private readonly LayerType layer;
        private readonly Texture2D texture;

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

        private float LayerDepth
        {
            get
            {
                switch (layer)
                {
                    case LayerType.First:
                        return LayerDepths.BG_PARALLAX_1;
                    case LayerType.Second:
                        return LayerDepths.BG_PARALLAX_2;
                    default:
                        return 200f;
                }                
            }
        }

        private Vector2 Pos
        {
            get { return new Vector2(-game.ViewPort.ViewPos.X*Speed, 0); }
        }

        public BgLayer(PlatformGame game, LayerType type, Texture2D texture)
        {
            this.game = game;
            layer = type;
            this.texture = texture;
        }

        public void Draw()
        {
            game.SpriteBatch.Draw(texture, Pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, LayerDepth);

            // TODO BDM (Backlog): Fix debug drawing for bg layers
            //if (CoreConfig.DebugModeEnabled)
            //{
            //    game.DebugDrawHelper.DrawDebugString(string.Format("L: {0}", Pos), new Vector2(640, 30));                
            //}
        } 
    }
}