using UnityEngine;
using System;

namespace LevelEditor3D.File
{
    [Serializable]
    public class AssetData
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public int assetBundle;
    }
}