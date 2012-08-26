﻿using GameEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC
{
    public class ResourcesHelper
    {
        public static CustomSpriteSheetDefinition BulletImpactSpriteSheet { get; private set; }

        public static Texture2D BgLayer1Texture { get; private set; }
        public static Texture2D BgLayer2Texture { get; private set; }
        public static Texture2D TilesetTexture { get; private set; }

        public static Texture2D BulletTexture { get; private set; }

        public static CustomSpriteSheetDefinition PlayerSpriteSheet { get; private set; }
        public static SoundEffect SpawnSound { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            BulletImpactSpriteSheet = new CustomSpriteSheetDefinition(content, "bullet-impact", new Rectangle(0, 0, 42, 29), 6);

            BgLayer1Texture = content.Load<Texture2D>("parallax-layer1");
            BgLayer2Texture = content.Load<Texture2D>("parallax-layer2");
            TilesetTexture = content.Load<Texture2D>("tileset");

            BulletTexture = content.Load<Texture2D>("bullet");

            PlayerSpriteSheet = new CustomSpriteSheetDefinition(content, "player", new Rectangle(0, 0, 32, 32), 8);
            SpawnSound = content.Load<SoundEffect>("testsound");
        }
    }
}