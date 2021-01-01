using LevelEditor3D.File;
using LevelEditor3D.Util;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveFileLoader
{
    public static void load(PaletteService paletteService)
    {
        var path = EditorUtility.OpenFilePanel(
                    "Open a SGON file?",
                    "",
                    "sgon");
        if (path.Length > 0)
        {
            if (File.Exists(path))
            {

                using (StreamReader sr = File.OpenText(path))
                {
                    string s;
                    Dictionary<int, int> assetBundleDictionary = new Dictionary<int, int>();
                    while ((s = sr.ReadLine()) != null)
                    {
                        //Read SaveFile object from file
                        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(s);
                        int counter = 0;

                        //Build dictionary for mapping of indexes
                        foreach (string assetBundleName in saveFile.header.assetBundles)
                        {
                            assetBundleDictionary.Add(counter, paletteService.getAssetBundleIndexByBundleName(assetBundleName));
                            counter++;
                        }

                        //Place assets in scene
                        foreach (Asset asset in saveFile.assets)
                        {
                            int bundleIndexInPalette = -1;
                            AssetData data = saveFile.header.getAssetData(asset.assetDataId);
                            if (assetBundleDictionary.TryGetValue(saveFile.header.getAssetBundleIndex(asset.assetDataId), out bundleIndexInPalette))
                            {
                                paletteService.placeAsset(asset.position, bundleIndexInPalette, paletteService.getAssetIndex(bundleIndexInPalette, data.name));
                            }
                        }
                    }
                    sr.Close();
                }
            }
        }
    }
}