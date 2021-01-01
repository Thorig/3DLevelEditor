using UnityEngine;
using System.Collections.Generic;
using System;

namespace LevelEditor3D.File
{
    [Serializable]
    public class Header
    {
        [SerializeField]
        public List<string> assetBundles = new List<string>();

        [SerializeField]
        public List<AssetData> assetData = new List<AssetData>();

        public int addAssetBundle(string name)
        {
            if (!assetBundles.Contains(name))
            {
                assetBundles.Add(name);
            }
           
            return getAssetBundleIndex(name);
        }

        public int getAssetBundleIndex(string name)
        {
            return assetBundles.IndexOf(name);
        }

        public int getAssetBundleIndex(int assetDataIndex)
        {
            return assetData[assetDataIndex].assetBundle;
        }

        public int getAssetDataIndex(AssetData data)
        {
            int index = -1;
            int counter = 0;
            foreach(AssetData aData in assetData)
            {
                if(aData.name.Equals(data.name) &&
                    aData.assetBundle == data.assetBundle)
                {
                    index  = counter;
                    break;
                }
                counter++;
            }
            return index;
        }

        public int getAssetDataIndex(string name, int assetBundleIndex)
        {
            int index = -1;
            int counter = 0;
            foreach (AssetData aData in assetData)
            {
                if (aData.name.Equals(name) &&
                    aData.assetBundle == assetBundleIndex)
                {
                    index = counter;
                    break;
                }
                counter++;
            }
            return index;
        }

        public int addAssetData(AssetData data)
        {
            if (getAssetDataIndex(data) == -1)
            {
                assetData.Add(data);
            }

            return getAssetDataIndex(data);
        }

        public AssetData getAssetData(int index)
        {
            return assetData[index];
        }

    }
}