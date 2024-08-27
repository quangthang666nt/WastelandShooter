using System.Collections.Generic;
using UnityEngine;

namespace Watermelon.LevelSystem
{
    [CreateAssetMenu(fileName = "World", menuName = "Content/New Level/World")]
    public class WorldData : ScriptableObject
    {
        [SerializeField] Sprite previewSprite;
        public Sprite PreviewSprite => previewSprite;

        [SerializeField] WorldPresetType obstaclesPresetType;
        public WorldPresetType ObstaclesPresetType => obstaclesPresetType;

        [SerializeField] LevelData[] levels;
        public LevelData[] Levels => levels;

        [SerializeField] LevelObstacle[] obstacles;
        public LevelObstacle[] Obstacles => obstacles;

        [SerializeField] LevelEnvironment[] environments;
        public LevelEnvironment[] Environments => environments;

        [SerializeField] GameObject pedestalPrefab;
        public GameObject PedestalPrefab => pedestalPrefab;

        private Dictionary<LevelObstaclesType, LevelObstacle> obstaclesDictionary;
        private Dictionary<LevelEnvironmentType, LevelEnvironment> environmentDictionary;

        public void Initialise()
        {
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i].Initialise(this);
            }

            obstaclesDictionary = new Dictionary<LevelObstaclesType, LevelObstacle>();
            for (int i = 0; i < obstacles.Length; i++)
            {
                obstaclesDictionary.Add(obstacles[i].Type, obstacles[i]);
            }

            environmentDictionary = new Dictionary<LevelEnvironmentType, LevelEnvironment>();
            for (int i = 0; i < environments.Length; i++)
            {
                environmentDictionary.Add(environments[i].Type, environments[i]);
            }
        }

        public void LoadWorld()
        {
            // creating obstacles pools
            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i].OnWorldLoaded();
            }

            // creating environment pools
            for (int i = 0; i < environments.Length; i++)
            {
                environments[i].OnWorldLoaded();
            }
        }

        public void UnloadWorld()
        {
            // releasing obstacles pools
            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i].OnWorldUnloaded();
            }

            // releasing environment pools
            for (int i = 0; i < environments.Length; i++)
            {
                environments[i].OnWorldUnloaded();
            }
        }

        public LevelObstacle GetObstacle(LevelObstaclesType levelObstaclesType)
        {
            return obstaclesDictionary[levelObstaclesType];
        }

        public LevelEnvironment GetEnvironment(LevelEnvironmentType levelEnvironmentType)
        {
            return environmentDictionary[levelEnvironmentType];
        }
    }
}
