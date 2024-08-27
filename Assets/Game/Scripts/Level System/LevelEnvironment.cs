using UnityEngine;

namespace Watermelon.LevelSystem
{
    [System.Serializable]
    public class LevelEnvironment
    {
        [SerializeField] GameObject prefab;
        public GameObject Prefab => prefab;

        [SerializeField] LevelEnvironmentType type;
        public LevelEnvironmentType Type => type;

        private Pool pool;
        public Pool Pool => pool;

        public void OnWorldLoaded()
        {
            pool = new Pool(new PoolSettings(prefab.name, prefab, 0, true));
        }

        public void OnWorldUnloaded()
        {
            pool.Clear();
            pool = null;
        }
    }
}