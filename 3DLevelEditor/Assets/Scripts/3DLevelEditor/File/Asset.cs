using System;
using UnityEngine;

namespace LevelEditor3D.File
{
    [Serializable]
    public struct Asset
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public int assetBundle;
        [SerializeField]
        public Vector3 position;
        [SerializeField]
        public Vector3 rotation;
    }
}