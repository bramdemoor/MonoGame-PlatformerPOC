//using System;
//using System.Collections.Generic;
//using System.Threading;
//using GameEngine;
//using GameEngine.GameObjects;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using PlatformerPOC.EventArgs;
//using PlatformerPOC.GameObjects;

//namespace PlatformerPOC
//{
//    public class PlayerManager
//    {
//        private readonly Dictionary<long, Player> players = new Dictionary<long, Player>();
//        private static long playerIdCounter;
//        private GameTimer hearbeatTimer;
//        private Player localPlayer;

//        public event EventHandler<PlayerStateChangedArgs> PlayerStateChanged;

//        public IEnumerable<Player> Players
//        {
//            get { return players.Values; }
//        }

//        public Player AddPlayer(long id, GameObjectState state, bool isLocal)
//        {
//            if (this.players.ContainsKey(id))
//            {
//                return this.players[id];
//            }

//            var player = new Player("p", id, state);

//            this.players.Add(player.Id, player);

//            if (isLocal)
//            {
//                this.localPlayer = player;
//            }

//            return player;
//        }

//        public Player AddPlayer(bool isLocal)
//        {
//            return AddPlayer(Interlocked.Increment(ref playerIdCounter), new GameObjectState(), isLocal);
//        }

//        public void Draw()
//        {
//            foreach (Player player in Players)
//            {
//                player.Draw();                
//            }
//        }

//        public Player GetPlayer(long id)
//        {
//            if (players.ContainsKey(id))
//            {
//                return players[id];
//            }

//            return null;
//        }

//        public void LoadContent(ContentManager contentManager)
//        {
//            this.hearbeatTimer = new GameTimer();
//        }

//        public bool PayerIsLocal(Player player)
//        {
//            return this.localPlayer != null && this.localPlayer.Id == player.Id;
//        }

//        public void RemovePlayer(long id)
//        {
//            if (this.players.ContainsKey(id))
//            {
//                this.players.Remove(id);
//            }
//        }

//        public void Update(GameTime gameTime)
//        {
//            if ((this.localPlayer != null))
//            {
//                bool velocityChanged = this.HandlePlayerMovement();

//                if (velocityChanged)
//                {
//                    this.OnPlayerStateChanged(this.localPlayer);
//                }
//            }

//            foreach (Player player in this.Players)
//            {
//                player.Update(gameTime);
//            }

//            if (SimpleGameEngine.Instance.IsHost && this.hearbeatTimer.Stopwatch(1000))
//            {
//                foreach (Player player in this.Players)
//                {
//                    this.OnPlayerStateChanged(player);
//                }
//            }
//        }

//        protected void OnPlayerStateChanged(Player player)
//        {
//            EventHandler<PlayerStateChangedArgs> playerStateChanged = this.PlayerStateChanged;
//            if (playerStateChanged != null)
//            {
//                playerStateChanged(this, new PlayerStateChangedArgs(player));
//            }
//        }

//        /// <summary>
//        /// The handle player movement.
//        /// </summary>
//        /// <returns>
//        /// The handle player movement.
//        /// </returns>
//        private bool HandlePlayerMovement()
//        {
//            bool velocityChanged = false;

//            this.localPlayer.SimulationState.Velocity = Vector2.Zero;

//            //if (this.inputManager.IsKeyDown(Keys.Up))
//            //{
//            //    this.localPlayer.SimulationState.Velocity += new Vector2(0, -1);
//            //    if (this.inputManager.IsKeyReleased(Keys.Up))
//            //    {
//            //        velocityChanged = true;
//            //    }
//            //}

//            //if (this.inputManager.IsKeyPressed(Keys.Up))
//            //{
//            //    this.localPlayer.SimulationState.Velocity = new Vector2(this.localPlayer.SimulationState.Velocity.X, 0);
//            //    velocityChanged = true;
//            //}

//            //if (this.inputManager.IsKeyDown(Keys.Down))
//            //{
//            //    this.localPlayer.SimulationState.Velocity += new Vector2(0, 1);
//            //    if (this.inputManager.IsKeyReleased(Keys.Down))
//            //    {
//            //        velocityChanged = true;
//            //    }
//            //}

//            //if (this.inputManager.IsKeyPressed(Keys.Down))
//            //{
//            //    this.localPlayer.SimulationState.Velocity = new Vector2(this.localPlayer.SimulationState.Velocity.X, 0);
//            //    velocityChanged = true;
//            //}

//            //if (this.inputManager.IsKeyDown(Keys.Left))
//            //{
//            //    this.localPlayer.SimulationState.Velocity += new Vector2(-1, 0);
//            //    if (this.inputManager.IsKeyReleased(Keys.Left))
//            //    {
//            //        velocityChanged = true;
//            //    }
//            //}

//            //if (this.inputManager.IsKeyPressed(Keys.Left))
//            //{
//            //    this.localPlayer.SimulationState.Velocity = new Vector2(0, this.localPlayer.SimulationState.Velocity.Y);
//            //    velocityChanged = true;
//            //}

//            //if (this.inputManager.IsKeyDown(Keys.Right))
//            //{
//            //    this.localPlayer.SimulationState.Velocity += new Vector2(1, 0);
//            //    if (this.inputManager.IsKeyReleased(Keys.Right))
//            //    {
//            //        velocityChanged = true;
//            //    }
//            //}

//            //if (this.inputManager.IsKeyPressed(Keys.Right))
//            //{
//            //    this.localPlayer.SimulationState.Velocity = new Vector2(0, this.localPlayer.SimulationState.Velocity.Y);
//            //    velocityChanged = true;
//            //}

//            //this.localPlayer.SimulationState.Velocity.Normalize();
//            //this.localPlayer.SimulationState.Velocity *= this.playerSpeed;

//            return velocityChanged;
//        }     
//    }
//}