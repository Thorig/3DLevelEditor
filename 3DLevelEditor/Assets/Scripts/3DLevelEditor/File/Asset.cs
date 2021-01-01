using System;
using UnityEngine;

namespace LevelEditor3D.File
{
    [Serializable]
    public struct Asset
    {

        //Remove after read of the save file is been upgraded with the header
        [SerializeField]
        public string name;
        [SerializeField]
        public int assetBundle;

        //Keep this for 1.0
        [SerializeField]
        public int assetDataId;
        [SerializeField]
        public Vector3 position;
        [SerializeField]
        public Vector3 rotation;
    }
}