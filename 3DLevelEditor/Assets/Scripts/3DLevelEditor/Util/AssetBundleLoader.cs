using System;
using UnityEngine;

namespace LevelEditor3D.Util
{
    public class AssetBundleLoader
    {
        private AssetBundle localAssetBundle;
         
        public void loadBundle(string bundleName)
        {
            AssetBundle.UnloadAllAssetBundles(true);

            localAssetBundle = AssetBundle.LoadFromFile(bundleName);
            if (localAssetBundle == null)
            {
                throw new Exception("Can't load asset bundle " + bundleName);
            }
        }

        public Material getMaterial(string assetName)
        {
            Material assetMaterial = localAssetBundle.LoadAsset<Material>(assetName);

            if (assetMaterial == null)
            {
                throw new Exception("Can't load material " + assetName + " from AssetBundle");
            }

            localAssetBundle.Unload(false);

            return assetMaterial;
        }

        public GameObject getGameObject(string assetName)
        {
            GameObject asset = localAssetBundle.LoadAsset<GameObject>(assetName);

            if(asset == null)
            {
                throw new Exception("Can't load GameObject " + assetName + " from AssetBundle");
            }

            GameObject sceneOBject = GameObject.Instantiate(asset);
            localAssetBundle.Unload(false);

            return sceneOBject;
        }

        public void cleanup()
        {
            if (localAssetBundle != null)
            {
                localAssetBundle?.Unload(true);
            }
        }
    }
}