using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor3D.Util
{
    public class PaletteService
    {
        public bool isLoaded { get; set; }
        private string manifestFile = "Assetbundlemanifest.xml";

        private ManifestReader manifestReader = new ManifestReader();
        private AssetBundleLoader assetBundleLoader = new AssetBundleLoader();
        private List<AssetBundle> assetBundles;

        public PaletteService()
        {
            isLoaded = false;
        }

        public void cleanup()
        {
            assetBundles.Clear();
            assetBundleLoader.cleanup(true);
        }

        public List<AssetBundle> getAssetBundles()
        {
            return assetBundles;
        }

        public void loadPalette()
        {
            if (!isLoaded)
            {
                isLoaded = true;

                assetBundles = manifestReader.parseManifest(manifestFile);

                try
                {
                    assetBundleLoader.loadBundle(assetBundles[0].location);
                    assetBundles[0].gameObjects = assetBundleLoader.getAllGameObjects();
                    int i = 0;
                    foreach (GameObject go in assetBundles[0].gameObjects)
                    {
                        for(i = 0; i < assetBundles[0].prefabList.Count; i++)
                        {
                            if(assetBundles[0].prefabList[i].name.Equals(go.name))
                            {
                                assetBundles[0].prefabList[i].index = i;
                                break;
                            }
                        }
                    }
                    i = 0;
                    foreach (Prefab prefab in assetBundles[0].prefabList)
                    {
                        assetBundles[0].prefabList[i].textNormal = assetBundleLoader.getTexture2D(prefab.iconNormal);
                        assetBundles[0].prefabList[i].textActive = assetBundleLoader.getTexture2D(prefab.iconActive);
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
                finally
                {
                    assetBundleLoader.cleanup(false);
                }
            }
        }

        public GameObject getPrefab(int selectedAssetBundle, int selectedAsset)
        {
            return assetBundles[selectedAssetBundle].gameObjects[assetBundles[selectedAssetBundle].prefabList[selectedAsset].index];
        }
    }
}