using UnityEngine;

namespace Watermelon.LevelSystem
{
    [System.Serializable]
    public class WorldPreset
    {
        [SerializeField] WorldPresetType type;
        public WorldPresetType Type => type;

        [SerializeField] LevelObstacle[] obstacles;
        public LevelObstacle[] Obstacles => obstacles;

        [SerializeField] LevelEnvironment[] environments;
        public LevelEnvironment[] Environments => environments;

        [SerializeField] GameObject pedestalPrefab;
        public GameObject PedestalPrefab => pedestalPrefab;

        public void Initialise()
        {
            
        }

       

       

        public void OnPresetLoaded()
        {
            
        }

        public void OnPresetUnloaded()
        {
           
        }
    }
}
