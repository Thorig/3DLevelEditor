using LevelEditor3D.File;
using LevelEditor3D.Util;
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

                    while ((s = sr.ReadLine()) != null)
                    {
                        Asset asset = JsonUtility.FromJson<Asset>(s);

                        if (asset.name != null && asset.assetBundle > -1)
                        {
                            paletteService.placeAsset(asset.position, asset.assetBundle, paletteService.getIndexForAsset(asset.assetBundle, asset.name));
                        }
                    }
                    sr.Close();
                }
            }
        }
    }
}