using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Drawing;

namespace PlatformerPOC.Domain
{
    public enum CharacterKeys
    {
        Blue, Pink, Yellow
    }

    public class Character
    {
        public CharacterKeys Key { get; private set; }

        public CustomSpriteSheetDefinition PlayerSpriteSheet { get; private set; }

        public Texture2D PlayerAvatar { get; private set; }

        private string avatarImageName
        {
            get { return "Characters/player-avatar"; }
        }

        private string spriteSheetName
        {
            get
            {
                switch (Key)
                {
                    case CharacterKeys.Blue:
                        return "Characters/player-blue";
                    case CharacterKeys.Pink:
                        return "Characters/player-pink";
                    case CharacterKeys.Yellow:
                        return "Characters/player-yellow";
                    default:
                        return "";
                }
            }
        }

        private Character()
        {
            
        }

        public static Character Create(ContentManager content, CharacterKeys key)
        {
            var c = new Character();

            c.Key = key;

            c.PlayerAvatar = content.Load<Texture2D>(c.avatarImageName);

            c.PlayerSpriteSheet = new CustomSpriteSheetDefinition(content, c.spriteSheetName, new Rectangle(0, 0, 32, 32), 8);

            return c;
        }
    }
}