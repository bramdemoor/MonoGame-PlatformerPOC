using System.Linq;
using GameEngine.Collision;
using GameEngine.GameObjects;
using GameEngine.Spritesheet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Concept;
using PlatformerPOC.Control;

namespace PlatformerPOC.GameObjects
{
    public class Player : PlatformGameObject
    {
        private const int MAX_LIFE = 100;
        private const int MOVE_SPEED = 5;
        private const float JUMP_FORCE = 6.2f;

        private Pistol weapon;
        private readonly CustomSpriteSheetInstance spriteSheetInstance;
        private IPlayerControlState playerInputState;

        public string Name { get; set; }
        public int Life { get; private set; }
        public Team Team { get; private set; }
        public Score Score { get; private set; }
        public Character Character { get; set; }

        public bool IsAlive
        {
            get { return Life > 0; }
        }

        public Color TextColor
        {
            get { return IsAlive ? Team.TeamColor : Team.TeamColorDark; }
        }

        public bool IsStandingOnSolid
        {
            get { return !game.LevelManager.CurrentLevel.IsPlaceFreeOfWalls(BoundingBox.BottomRectangle); }
        }

        private bool WantsToMoveHorizontally
        {
            get { return playerInputState.IsMoveLeftPressed || playerInputState.IsMoveRightPressed; }
        }

        public Player(PlatformGame game, string name, long id, GameObjectState gameObjectState): base(game)
        {
            BoundingBox = new CustomBoundingBox();

            Name = name;

            Score = new Score();
            Team = new NeutralTeam();

            Character = game.ResourcesHelper.Characters.First();

            spriteSheetInstance = new CustomSpriteSheetInstance(game, Character.PlayerSpriteSheet, 3);
        }


        #region Lifecycle

        public void Spawn(Vector2 spawnPoint)
        {
            Position = spawnPoint;

            weapon = new Pistol(game, this);

            Life = MAX_LIFE;

            PlaySound(game.ResourcesHelper.SpawnSound);
        }

        public void DoDamage(int damage)
        {
            if (!IsAlive) return;

            Life -= damage;

            if (!IsAlive)
            {
                Die();
            }
        }

        private void Die()
        {
            Score.MarkDeath();
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

            if (!game.LevelManager.CurrentLevel.IsPlaceFreeOfWalls(BoundingBox.FullRectangle))
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
                if (game.LevelManager.CurrentLevel.IsPlaceFreeOfWalls(BoundingBox.RightRectangle))
                {
                    Position = new Vector2(Position.X + Velocity.X, Position.Y);
                }
            }

            if (Velocity.X < 0)
            {
                if (game.LevelManager.CurrentLevel.IsPlaceFreeOfWalls(BoundingBox.LeftRectangle))
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
                if (!game.LevelManager.CurrentLevel.IsPlaceFreeOfWalls(BoundingBox.TopRectangle))
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
                weapon.Shoot();
            }
        }

        #endregion


        #region Drawing

        public override void Draw()
        {
            spriteSheetInstance.Draw(PositionRelativeToView, DrawEffect, LayerDepths.GAMEOBJECTS);

            var displayText = string.Format("{0}", Name);

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
                if (game.LevelManager.CurrentLevel.IsPlaceFreeOfWalls(newRect))
                {
                    Velocity = new Vector2(Velocity.X, -JUMP_FORCE);
                }
            }
        }

        private void ReverseMidAir()
        {
            // New movement concept. "Stop jumping and go down"
            // Alias: "brake mid-air"

            if (Velocity.Y < 0) Velocity = new Vector2(Velocity.X, 0);
        }

        #endregion


        #region Meta actions

        public void SwitchTeam(Team newTeam)
        {
            Team = newTeam;
        }

        #endregion

    }
}