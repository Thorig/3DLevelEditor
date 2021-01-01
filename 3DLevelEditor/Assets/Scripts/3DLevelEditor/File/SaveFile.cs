using UnityEngine;
using System;
using System.Collections.Generic;

namespace LevelEditor3D.File
{
    [Serializable]
    public class SaveFile
    {
        [SerializeField]
        public Header header = new Header();

        [SerializeField]
        public List<Asset> assets = new List<Asset>();
    }
}