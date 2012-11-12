using GameEngine.Collision;
using GameEngine.GameObjects;
using GameEngine.Spritesheet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Control;

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
                return !game.Level.IsPlaceFreeOfWalls(BoundingBox.BottomRectangle);
            }
        }

        private readonly CustomSpriteSheetInstance spriteSheetInstance;

        private IPlayerControlState playerInputState;

        private bool WantsToMoveHorizontally
        {
            get { return playerInputState.IsMoveLeftPressed || playerInputState.IsMoveRightPressed; }
        }

        public Player(PlatformGame game, string name, long id, GameObjectState gameObjectState): base(game)
        {
            BoundingBox = new CustomBoundingBox();

            spriteSheetInstance = new CustomSpriteSheetInstance(game, game.ResourcesHelper.PlayerSpriteSheet, 3);

            Name = name;           
        }

        public void DoDamage(int damage)
        {
            if(IsAlive)
            {
                Life -= damage;
            }
        }

        public void Spawn(Vector2 spawnPoint)
        {
            Position = spawnPoint;

            Life = MAX_LIFE;

            PlaySound(game.ResourcesHelper.SpawnSound);
        }

        public void Update()
        {
            // FYI: UpdateBoundingBox called multiple times to prevent some nasty bugs

            UpdateBoundingBox();

            if(IsAlive)
            {
                ApplyInput();
            }
            
            if (!game.Level.IsPlaceFreeOfWalls(BoundingBox.FullRectangle))
            {
                Velocity = new Vector2(Velocity.X, 0);
            }

            VerticalMovement();

            UpdateBoundingBox();

            HorizontalMovement();
            

            if (IsAlive && WantsToMoveHorizontally)
            {
                spriteSheetInstance.LoopWithReverse();
            }

            UpdateBoundingBox();
        }

        private void UpdateBoundingBox()
        {
            BoundingBox.SetFullRectangle(Position, spriteSheetInstance.SpriteSheetDefinition.SpriteDimensions, Velocity, 4, 4, 4, 0);
        }

        private void HorizontalMovement()
        {
            if(Velocity.X > 0)
            {
                if(game.Level.IsPlaceFreeOfWalls(BoundingBox.RightRectangle))
                {
                    Position = new Vector2(Position.X + Velocity.X, Position.Y);
                }
            }

            if (Velocity.X < 0)
            {
                if (game.Level.IsPlaceFreeOfWalls(BoundingBox.LeftRectangle))
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
                if (!game.Level.IsPlaceFreeOfWalls(BoundingBox.TopRectangle))
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
            if (!InView) return;
                        
                spriteSheetInstance.Draw(PositionRelativeToView, DrawEffect, LayerDepths.GAMEOBJECTS);

                game.SpriteBatch.DrawString(game.ResourcesHelper.DefaultFont, Name, PositionRelativeToView, Color.White, 0, new Vector2(0, 30), 0.65f, SpriteEffects.None, LayerDepths.TEXT);                        
        }

        public override void DrawDebug()
        {
            var rel = ViewPort.GetRelativeCoords(BoundingBox.FullRectangle);

            game.DebugDrawHelper.DrawBorder(rel, 1, Color.Pink);
        }

        public void HandleInput(IPlayerControlState playerInputState)
        {
            this.playerInputState = playerInputState;
        }

        private void Jump()
        {
            if (IsStandingOnSolid)
            {
                // if enough space above to jump
                //Offset(0,-10)
                var newRect = new Rectangle(BoundingBox.TopRectangle.X, BoundingBox.TopRectangle.Y - 7, BoundingBox.TopRectangle.Width, 1);
                if (game.Level.IsPlaceFreeOfWalls(newRect))
                {
                    Velocity = new Vector2(Velocity.X, -6f);        
                }
            }
        }

        private void Shoot()
        {
            var bullet = new Bullet(game, this, Position + new Vector2(30 * HorizontalDirection, 12), HorizontalDirection);
            game.AddObject(bullet);            
        }
    }
}