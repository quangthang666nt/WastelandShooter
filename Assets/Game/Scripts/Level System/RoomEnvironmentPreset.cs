using UnityEngine;

namespace Watermelon.LevelSystem
{
    [System.Serializable]
    public class RoomEnvironmentPreset 
    {
        [SerializeField] string name;
        [SerializeField] EnvironmentEntityData[] environmentEntities;
        [SerializeField] Vector3 spawnPos;
        [SerializeField] Vector3 exitPointPos;
    }
}