using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LevelEditor3D.Util
{
    public class PaletteService
    {
        private GameObject parent = null;

        public bool isLoaded { get; set; }
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

        public void FromManifestFile(string manifestFile)
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
                    int index = 0;
                    foreach (GameObject go in assetBundles[0].gameObjects)
                    {
                        for (i = 0; i < assetBundles[0].prefabList.Count; i++)
                        {
                            if (assetBundles[0].prefabList[i].name.Equals(go.name))
                            {
                                assetBundles[0].prefabList[i].index = index;
                                break;
                            }
                        }
                        index++;
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

        public int getAssetBundleIndexByBundleName(string name)
        {
            int assetBundleId = 0;

            foreach (AssetBundle assetBundle in assetBundles)
            {
                if(assetBundle.name.Equals(name))
                {
                    break;
                }
                assetBundleId++;
            }
            if(assetBundleId == assetBundles.Count)
            {
                assetBundleId = -1;
            }

            return assetBundleId;
        }

        public AssetBundle getAssetBundleByPrefabName(string name)
        {
            AssetBundle bundle = null;

            foreach (AssetBundle assetBundle in assetBundles)
            {
                foreach (Prefab prefab in assetBundle.prefabList)
                {
                    if (prefab.name.Equals(name))
                    {
                        bundle = assetBundle;
                        break;
                    }
                }
                if (bundle != null)
                {
                    break;
                }
            }

            return bundle;
        }

        public int getAssetIndex(int selectedAssetBundle, string assetName)
        {
            int assetId = -1;

            foreach (Prefab prefab in assetBundles[selectedAssetBundle].prefabList)
            {
                if (prefab.name.Equals(assetName))
                {
                    assetId = prefab.index;
                    break;
                }
            }

            return assetId;
        }

        public void placeAsset(Vector3 position, int selectedAssetBundle, int selectedAsset)
        {
            if(parent == null)
            {
                parent = GameObject.FindGameObjectWithTag("World");
                if(parent == null)
                {
                    parent = new GameObject();
                }
                parent.name = "World";
                parent.tag = "World";
                parent.transform.position = Vector3.zero;
                EditorUtility.SetDirty(parent);
            }
            GameObject sceneOBject = GameObject.Instantiate(assetBundles[selectedAssetBundle].gameObjects[selectedAsset]);
            sceneOBject.transform.position = position;
            sceneOBject.transform.parent = parent.transform;
            sceneOBject.AddComponent<BoxCollider>();

            EditorUtility.SetDirty(sceneOBject);
        }
    }
}