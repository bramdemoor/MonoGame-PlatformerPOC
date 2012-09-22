using System.Collections.Generic;
using GameEngine.GameObjects;
using GameEngine.Helpers;
using Microsoft.Xna.Framework.Content;

namespace GameEngine
{
    public abstract class SimpleGameBase
    {
        private readonly List<BaseGameObject> _gameObjects;
        public IEnumerable<BaseGameObject> GameObjects
        {
            get { return _gameObjects; }
        }

        private readonly List<BaseGameObject> gameObjectsToAdd = new List<BaseGameObject>();
        private readonly List<BaseGameObject> gameObjectsToDelete = new List<BaseGameObject>();

        public DebugDrawHelper DebugDrawHelper { get; private set; }

        public ViewPort ViewPort { get; set; }

        public SimpleGameBase()
        {
            _gameObjects = new List<BaseGameObject>();

            DebugDrawHelper = new DebugDrawHelper();
        }

        public virtual void LoadContent(ContentManager content)
        {
            DebugDrawHelper.LoadContent();
        }

        /// <summary>
        /// Add object to the update/draw list
        /// </summary>        
        public void AddObject(BaseGameObject baseGameObject)
        {
            gameObjectsToAdd.Add(baseGameObject);
        }

        /// <summary>
        /// Delete object from the update/draw list
        /// </summary>
        public void DeleteObject(BaseGameObject baseGameObject)
        {
            gameObjectsToDelete.Add(baseGameObject);
        }

        /// <summary>
        /// Clean up objects to delete, and introduce new objects
        /// </summary>
        public void DoHouseKeeping()
        {
            foreach (var baseGameObject in gameObjectsToAdd)
            {
                _gameObjects.Add(baseGameObject);
            }

            gameObjectsToAdd.Clear();

            foreach (var baseGameObject in gameObjectsToDelete)
            {
                _gameObjects.Remove(baseGameObject);
            }
            
            gameObjectsToDelete.Clear();
        }
    }
}