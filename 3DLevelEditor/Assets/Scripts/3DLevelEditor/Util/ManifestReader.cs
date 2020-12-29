using UnityEngine;
using System.Xml;
using System.Collections.Generic;

namespace LevelEditor3D.Util
{
    public class ManifestReader
    {
        public List<AssetBundle> parseManifest(string manifest)
        {
            List<AssetBundle> assetsBundles = new List<AssetBundle>();
                        
            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(manifest);
            
            XmlNodeList elemList = doc.GetElementsByTagName("assetsbundle");

            //Loop through all assetbundles
            for (int i = 0; i < elemList.Count; i++)
            {
                AssetBundle assetsBundle = new AssetBundle();

                XmlAttributeCollection assetsBundleAttrs = elemList[i].Attributes;
                for (int x = 0; x < assetsBundleAttrs.Count; x++)
                {
                    if (assetsBundleAttrs[x].Name.Equals("location"))
                    {
                        assetsBundle.location = assetsBundleAttrs[x].Value;
                    }
                    if (assetsBundleAttrs[x].Name.Equals("filename"))
                    {
                        assetsBundle.name = assetsBundleAttrs[x].Value;
                    }
                }

                XmlNodeList prefabsList = elemList[i].ChildNodes;

                for (int iPrefab = 0; iPrefab < prefabsList.Count; iPrefab++)
                {
                    Prefab prefab = new Prefab();

                    XmlAttributeCollection prefabAttrs = prefabsList[iPrefab].Attributes;
                    for (int x = 0; x < prefabAttrs.Count; x++)
                    {
                        if(prefabAttrs[x].Name.Equals("filename"))
                        {
                            prefab.name = prefabAttrs[x].Value;
                        }
                        if (prefabAttrs[x].Name.Equals("groupname"))
                        {
                            prefab.group = prefabAttrs[x].Value;
                        }
                    }
                    if (prefabsList[iPrefab].HasChildNodes)
                    {
                        XmlNodeList prefabChildList = prefabsList[iPrefab].ChildNodes;

                        for (int y = 0; y < prefabChildList.Count; y++)
                        {
                            if (prefabChildList[y].Name.CompareTo("icon") == 0)
                            {
                                XmlAttributeCollection childAttrs = prefabChildList[y].Attributes;
                                for (int z = 0; z < childAttrs.Count; z++)
                                {
                                    if (childAttrs[z].Name.Equals("normal"))
                                    {
                                        prefab.iconNormal = childAttrs[z].Value;
                                    }
                                    if (childAttrs[z].Name.Equals("active"))
                                    {
                                        prefab.iconActive = childAttrs[z].Value;
                                    }
                                }
                            }
                        }
                    }
                    assetsBundle.prefabList.Add(prefab);
                }
                assetsBundles.Add(assetsBundle);
            }
            return assetsBundles;
        }
    }
}