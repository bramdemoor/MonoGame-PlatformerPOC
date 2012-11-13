using System;
using GameEngine.Collision;
using GameEngine.GameObjects;
using GameEngine.Spritesheet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Control;

namespace PlatformerPOC.GameObjects
{
    public enum PlayerTeams
    {
        NotSet = -1,
        Red = 1,
        Blue = 2
    }

    public class Player : PlatformGameObject
    {
        #region Data

        private const int MAX_LIFE = 100;
        private const int MOVE_SPEED = 5;
        private const float jumpForce = 6.2f;

        public string Name { get; set; }

        public int Life { get; private set; }
        public int Wins { get; set; }
        public int Deaths { get; set; }
        public PlayerTeams Team { get; set; }

        public Color TeamColor
        {
            get
            {
                switch (Team)
                {
                    case PlayerTeams.Red:
                        return Color.Red;
                    case PlayerTeams.Blue:
                        return Color.Blue;
                    default:
                        return Color.White;
                }
            }
        }

        public Color TeamColorDark
        {
            get
            {
                switch (Team)
                {
                    case PlayerTeams.Red:
                        return Color.DarkRed;
                    case PlayerTeams.Blue:
                        return Color.DarkBlue;
                    default:
                        return Color.DarkGray;
                }
            }
        }

        public bool IsAlive
        {
            get { return Life > 0; }
        }

        private Color TextColor
        {
            get { return IsAlive ? TeamColor : TeamColorDark; }
        }

        public bool IsStandingOnSolid
        {
            get { return !game.Level.IsPlaceFreeOfWalls(BoundingBox.BottomRectangle); }
        }

        private readonly CustomSpriteSheetInstance spriteSheetInstance;

        private IPlayerControlState playerInputState;

        private bool WantsToMoveHorizontally
        {
            get { return playerInputState.IsMoveLeftPressed || playerInputState.IsMoveRightPressed; }
        }

        #endregion


        public Player(PlatformGame game, string name, long id, GameObjectState gameObjectState): base(game)
        {
            BoundingBox = new CustomBoundingBox();

            spriteSheetInstance = new CustomSpriteSheetInstance(game, game.ResourcesHelper.PlayerSpriteSheet, 3);

            Name = name;           
        }


        #region Lifecycle

        public void DoDamage(int damage)
        {
            if (IsAlive)
            {
                Life -= damage;

                if (!IsAlive)
                {
                    Die();
                }
            }
        }

        private void Die()
        {
            Deaths++;
        }

        public void Spawn(Vector2 spawnPoint)
        {
            Position = spawnPoint;

            Life = MAX_LIFE;

            PlaySound(game.ResourcesHelper.SpawnSound);
        }

        #endregion


        #region Loop

        public void Update()
        {
            // FYI: UpdateBoundingBox called multiple times to prevent some nasty bugs

            UpdateBoundingBox();

            if (IsAlive)
            {
                ApplyInput();
            }

            if (!game.Level.IsPlaceFreeOfWalls(BoundingBox.FullRectangle))
            {
                Velocity = new Vector2(Velocity.X, 0);
            }

            VerticalMovement();

            UpdateBoundingBox();


            if (IsAlive)
            {
                HorizontalMovement();
            }

            if (IsAlive && WantsToMoveHorizontally)
            {
                spriteSheetInstance.LoopWithReverse();
            }

            UpdateBoundingBox();
        }

        private void UpdateBoundingBox()
        {
            BoundingBox.SetFullRectangle(Position, spriteSheetInstance.SpriteSheetDefinition.SpriteDimensions, Velocity,
                                         4, 4, 4, 0);
        }

        private void HorizontalMovement()
        {
            if (Velocity.X > 0)
            {
                if (game.Level.IsPlaceFreeOfWalls(BoundingBox.RightRectangle))
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
            if (Velocity.Y < 0)
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

        #endregion


        #region Input

        public void HandleInput(IPlayerControlState playerInputState)
        {
            this.playerInputState = playerInputState;
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

            if (playerInputState.IsMoveDownPressed)
            {
                ReverseMidAir();
            }

            if (playerInputState.IsActionPressed)
            {
                Shoot();
            }
        }

        #endregion


        #region Drawing

        public override void Draw()
        {
            if (!InView) return;

            spriteSheetInstance.Draw(PositionRelativeToView, DrawEffect, LayerDepths.GAMEOBJECTS);

            var displayText = string.Format("{0} ({1}/{2})", Name, Wins, Deaths);

            game.SpriteBatch.DrawString(game.ResourcesHelper.DefaultFont, displayText, PositionRelativeToView, TextColor, 0, new Vector2(0, 30), 0.65f, SpriteEffects.None, LayerDepths.TEXT);
        }

        public override void DrawDebug()
        {
            var rel = ViewPort.GetRelativeCoords(BoundingBox.FullRectangle);

            game.DebugDrawHelper.DrawBorder(rel, 1, Color.Pink);
        }

        #endregion


        #region Actions

        private void Jump()
        {
            if (IsStandingOnSolid)
            {
                // if enough space above to jump
                //Offset(0,-10)
                var newRect = new Rectangle(BoundingBox.TopRectangle.X, BoundingBox.TopRectangle.Y - 7,
                                            BoundingBox.TopRectangle.Width, 1);
                if (game.Level.IsPlaceFreeOfWalls(newRect))
                {
                    Velocity = new Vector2(Velocity.X, -jumpForce);
                }
            }
        }

        private void ReverseMidAir()
        {
            // New movement concept. "Stop jumping and go down"
            // Alias: "brake mid-air"

            if (Velocity.Y < 0) Velocity = new Vector2(Velocity.X, 0);
        }

        private void Shoot()
        {
            var bullet = new Bullet(game, this, Position + new Vector2(30*HorizontalDirection, 12), HorizontalDirection);
            game.AddObject(bullet);
        }

        #endregion

    }
}