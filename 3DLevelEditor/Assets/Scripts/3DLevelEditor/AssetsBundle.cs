using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor3D
{
    public class AssetsBundle
    {
        public string name;
        public string location;
        public List<Prefab> prefabList = new List<Prefab>();
        public GameObject[] gameObjects;
    }
}