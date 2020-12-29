using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor3D.Util
{
    public class AssetBundleLoader
    {
        private AssetBundle localAssetBundle;
         
        public List<string> loadBundle(string bundleName)
        {
            Debug.Log(bundleName);
            AssetBundle.UnloadAllAssetBundles(true);

            localAssetBundle = AssetBundle.LoadFromFile(bundleName);
            if (localAssetBundle == null)
            {
                throw new Exception("Can't load asset bundle " + bundleName);
            }
            string tmp = "";
            string[] names = localAssetBundle. GetAllAssetNames();
            List<string> listWithNames = new List<string>();

            foreach (string name in names)
            {
                int i = name.LastIndexOf('/');
                if(i < 0)
                {
                    i = name.LastIndexOf('\\');
                }
                tmp = name.Substring(i + 1);
                i = tmp.LastIndexOf('.');
                tmp = tmp.Substring(0, i);
                listWithNames.Add(tmp);
            }

            GameObject[] gList = localAssetBundle.LoadAllAssets<GameObject>();
            foreach (GameObject g in gList)
            {
                Debug.Log(g.name);
            }
             
            return listWithNames;
        }

        public GameObject[] getAllGameObjects()
        {
            GameObject[] gameObjects = localAssetBundle.LoadAllAssets<GameObject>();

            if (gameObjects == null)
            {
                throw new Exception("Can't load GameObjects from AssetBundle");
            }

            return gameObjects;
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

        public Texture2D getTexture2D(string assetName)
        {
            Texture2D asset = localAssetBundle.LoadAsset<Texture2D>(assetName);

            if (asset == null)
            {
                throw new Exception("Can't load GameObject " + assetName + " from AssetBundle");
            }
                        
            return asset;
        }

        public GameObject getGameObject(string assetName)
        {
            GameObject asset = localAssetBundle.LoadAsset<GameObject>(assetName);

            if(asset == null)
            {
                throw new Exception("Can't load GameObject " + assetName + " from AssetBundle");
            }

            localAssetBundle.Unload(false);

            return asset;
        }

        public void cleanup(bool cleanup)
        {
            if (localAssetBundle != null)
            {
                localAssetBundle?.Unload(cleanup);
            }
        }
    }
}