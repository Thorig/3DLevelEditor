using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor3D.Util
{
    public class PaletteService
    {
        public bool isLoaded { get; set; }
        private string manifestFile = "Assetsbundlemanifest.xml";

        private ManifestReader manifestReader = new ManifestReader();
        private AssetBundleLoader assetBundleLoader = new AssetBundleLoader();
        private List<AssetsBundle> assetsBundles;

        public PaletteService()
        {
            isLoaded = false;
        }

        public void cleanup()
        {
            assetsBundles.Clear();
            assetBundleLoader.cleanup(true);
        }

        public List<AssetsBundle> getAssetsBundles()
        {
            return assetsBundles;
        }

        public void loadPalette()
        {
            if (!isLoaded)
            {
                isLoaded = true;

                assetsBundles = manifestReader.parseManifest(manifestFile);

                try
                {
                    assetBundleLoader.loadBundle(assetsBundles[0].location);
                    assetsBundles[0].gameObjects = assetBundleLoader.getAllGameObjects();
                    int i = 0;
                    foreach (GameObject go in assetsBundles[0].gameObjects)
                    {
                        for(i = 0; i < assetsBundles[0].prefabList.Count; i++)
                        {
                            if(assetsBundles[0].prefabList[i].name.Equals(go.name))
                            {
                                assetsBundles[0].prefabList[i].index = i;
                                break;
                            }
                        }
                    }
                    i = 0;
                    foreach (Prefab prefab in assetsBundles[0].prefabList)
                    {
                        assetsBundles[0].prefabList[i].textNormal = assetBundleLoader.getTexture2D(prefab.iconNormal);
                        assetsBundles[0].prefabList[i].textActive = assetBundleLoader.getTexture2D(prefab.iconActive);
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

        public GameObject getPrefab(int selectedAssetsBundle, int selectedAsset)
        {
            return assetsBundles[selectedAssetsBundle].gameObjects[assetsBundles[selectedAssetsBundle].prefabList[selectedAsset].index];
        }
    }
}