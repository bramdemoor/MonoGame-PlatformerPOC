﻿using GameEngine;
using GameEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PlatformerPOC.GameObjects;

namespace PlatformerPOC.Screens
{
    public class GameplayScreen : SimpleScreenBase
    {
        public override void Draw(GameTime gameTime)
        {
            PlatformGame.Instance.Level.Draw();
            PlatformGame.Instance.LocalPlayer.Draw();

            foreach (var bullet in PlatformGame.Instance.Bullets)
            {
                bullet.Draw();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                SimpleGameEngine.Instance.ActiveScreen = new LobbyScreen();                
            }

            PlatformGame.Instance.LocalPlayer.HandleInput(new PlayerKeyboardState(Keyboard.GetState()));
            PlatformGame.Instance.LocalPlayer.Update();

            foreach (var bullet in PlatformGame.Instance.Bullets)
            {
                bullet.Update(gameTime);
            }
        }
    }
}