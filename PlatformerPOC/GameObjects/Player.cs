using GameEngine;
using GameEngine.GameObjects;
using GameEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.GameObjects
{
    public class Player : PlatformGameObject
    {
        private const int MAX_LIFE = 100;
        private const int MOVE_SPEED = 5;

        public string Name { get; set; }

        public int Life { get; set; }

        public bool IsAlive
        {
            get { return Life > 0; }
        }

        public bool IsStandingOnSolid
        {
            get
            {
                return !PlatformGame.Instance.Level.IsPlaceFree(BoundingBox.BottomRectangle);
            }
        }

        private readonly CustomSpriteSheetInstance spriteSheetInstance;

        private IPlayerControlState playerInputState;

        private bool WantsToMoveHorizontally
        {
            get { return playerInputState.IsMoveLeftPressed || playerInputState.IsMoveRightPressed; }
        }

        public Player(string name, long id, GameObjectState gameObjectState)
        {
            BoundingBox = new CustomBoundingBox();
            
            spriteSheetInstance = new CustomSpriteSheetInstance(ResourcesHelper.PlayerSpriteSheet, 3);

            this.Name = name;

            Spawn();            
        }

        public void DoDamage(int damage)
        {
            if(IsAlive)
            {
                Life -= damage;
            }
        }

        public void Spawn()
        {


            Life = MAX_LIFE;

            PlaySound(ResourcesHelper.SpawnSound);            

            Position = PlatformGame.Instance.Level.GetNextFreeSpawnPoint();
            UpdateBoundingBox();
        }

        public void Update()
        {
            UpdateBoundingBox();

            if(IsAlive)
            {
                ApplyInput();
            }
            
            if (!PlatformGame.Instance.Level.IsPlaceFree(BoundingBox.FullRectangle))
            {
                Velocity = new Vector2(Velocity.X, 0);
            }

            VerticalMovement();
            HorizontalMovement();
            

            if (IsAlive && WantsToMoveHorizontally)
            {
                spriteSheetInstance.LoopWithReverse();
            }            
        }

        private void UpdateBoundingBox()
        {
            // TODO BDM: Refactor
            BoundingBox.SetFullRectangle(new Rectangle((int)Position.X + 4, (int)Position.Y + 4, spriteSheetInstance.SpriteSheetDefinition.SpriteDimensions.Width - 8, spriteSheetInstance.SpriteSheetDefinition.SpriteDimensions.Height - 2), Velocity);
        }

        private void HorizontalMovement()
        {
            if(Velocity.X > 0)
            {
                if(PlatformGame.Instance.Level.IsPlaceFree(BoundingBox.RightRectangle))
                {
                    Position = new Vector2(Position.X + Velocity.X, Position.Y);
                }
            }

            if (Velocity.X < 0)
            {
                if (PlatformGame.Instance.Level.IsPlaceFree(BoundingBox.LeftRectangle))
                {
                    Position = new Vector2(Position.X + Velocity.X, Position.Y);
                }
            }        
        }

        private void VerticalMovement()
        {
            // TOP CHECK
            if(Velocity.Y < 0)
            {
                if (!PlatformGame.Instance.Level.IsPlaceFree(BoundingBox.TopRectangle))
                {
                    Velocity = new Vector2(Velocity.X, 0);
                }
            }

            if (!IsStandingOnSolid)
            {
                Velocity += new Vector2(0, 0.2f);
            }
            else
            {
                if (Velocity.Y > 0)
                {
                    Velocity = new Vector2(Velocity.X, 0);
                }
            }

             Position = new Vector2(Position.X, Position.Y + Velocity.Y);
        }

        private void ApplyInput()
        {
            if (playerInputState.IsMoveLeftPressed)
            {
                Velocity = new Vector2(- MOVE_SPEED, Velocity.Y);
                HorizontalDirection = -1;
            }
            else
            {
                if (playerInputState.IsMoveRightPressed)
                {
                    Velocity = new Vector2(MOVE_SPEED, Velocity.Y);
                    HorizontalDirection = 1;
                }
                else
                {
                    Velocity = new Vector2(0, Velocity.Y);
                }
            }

            if (playerInputState.IsMoveUpPressed)
            {
                Jump();
            }

            if(playerInputState.IsMoveDownPressed)
            {
                ReverseMidAir();
            }

            if (playerInputState.IsActionPressed)
            {
                Shoot();
            }            
        }

        private void ReverseMidAir()
        {
            // New movement concept. "Stop jumping and go down"
            // Alias: "brake mid-air"

            if(Velocity.Y < 0) Velocity = new Vector2(Velocity.X, 0);
        }

        public override void Draw()
        {
            if (ViewPort.IsObjectInArea(BoundingBox.FullRectangle))
            {
                var relativePos = ViewPort.GetRelativeCoords(Position);

                spriteSheetInstance.Draw(relativePos, DrawEffect, LayerDepths.GAMEOBJECTS);

                SimpleGameEngine.Instance.spriteBatch.DrawString(ResourcesHelper.DefaultFont, Name, relativePos, Color.White, 0, new Vector2(0, 30), 0.65f, SpriteEffects.None, LayerDepths.GAMEOBJECTS);
            }            
        }

        public override void DrawDebug()
        {
            var rel = ViewPort.GetRelativeCoords(BoundingBox.FullRectangle);

            PlatformGame.Instance.DebugDrawHelper.DrawBorder(SpriteBatch, rel, 1, Color.Pink);
        }

        public void HandleInput(IPlayerControlState playerInputState)
        {
            this.playerInputState = playerInputState;
        }

        private void Jump()
        {
            if (IsStandingOnSolid)
            {
                Velocity = new Vector2(Velocity.X, -6f);
            }
        }

        private void Shoot()
        {
            var bullet = new Bullet(this, Position + new Vector2(30 * HorizontalDirection, 12), HorizontalDirection);
            PlatformGame.Instance.AddObject(bullet);            
        }
    }
}