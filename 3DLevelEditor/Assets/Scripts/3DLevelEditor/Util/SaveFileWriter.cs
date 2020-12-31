using LevelEditor3D.File;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LevelEditor3D.Util
{
    public class SaveFileWriter
    {
        public static void saveCurrentScene(PaletteService paletteService)
        {
            var path = EditorUtility.SaveFilePanel(
                        "Save scene as SGON?",
                        "",
                        ".sgon",
                        "sgon");
            if (path.Length > 0)
            {
                GameObject world = GameObject.FindGameObjectWithTag("World");
                if (world != null)
                {
                    using (StreamWriter writer = new StreamWriter(path, false))
                    {
                        foreach (Transform child in world.transform)
                        {
                            Asset asset = new Asset();

                            asset.name = child.gameObject.name;
                            asset.name = asset.name.Substring(0, asset.name.LastIndexOf('('));
                            asset.position = child.position;
                            asset.rotation = child.eulerAngles;
                            asset.assetBundle = paletteService.getAssetBundleId(asset.name);

                            writer.WriteLine(JsonUtility.ToJson(asset));
                        }
                        writer.Close();
                    }
                }
            }
        }
    }
}