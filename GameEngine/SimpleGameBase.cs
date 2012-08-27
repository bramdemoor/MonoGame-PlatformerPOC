using System.Collections.Generic;
using GameEngine.GameObjects;
using Microsoft.Xna.Framework.Content;

namespace GameEngine
{
    public abstract class SimpleGameBase
    {
        private List<BaseGameObject> _gameObjects;
        public IEnumerable<BaseGameObject> GameObjects
        {
            get { return _gameObjects; }
        }

        private readonly List<BaseGameObject> gameObjectsToAdd = new List<BaseGameObject>();
        private readonly List<BaseGameObject> gameObjectsToDelete = new List<BaseGameObject>();

        public SimpleGameBase()
        {
            _gameObjects = new List<BaseGameObject>();
        }

        public abstract void LoadContent(ContentManager content);

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