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
                    SaveFile saveFile = new SaveFile();
                    string assetName = "";
                    int indexAssetData = -1;

                    foreach (Transform child in world.transform)
                    {
                        assetName = child.gameObject.name.Substring(0, child.gameObject.name.LastIndexOf('('));

                        //Get AssetBundle from Palette
                        AssetBundle tmpBundle = paletteService.getAssetBundleByPrefabName(assetName);

                        //Add AssetBundle to header if needed
                        int indexBundle = saveFile.header.addAssetBundle(tmpBundle.name);

                        //Check if AssetData is know in 
                        indexAssetData = saveFile.header.getAssetDataIndex(assetName, indexBundle);

                        if (indexAssetData == -1)
                        {
                            //Add AssetData if needed
                            AssetData data = new AssetData();
                            data.name = assetName;
                            data.assetBundle = indexBundle;
                            indexAssetData = saveFile.header.addAssetData(data);
                        }

                        //Add Asset to Asset list
                        Asset asset = new Asset();
                        asset.assetDataId = indexAssetData;
                        asset.position = child.position;
                        asset.rotation = child.eulerAngles;

                        saveFile.assets.Add(asset);
                    }

                    using (StreamWriter writer = new StreamWriter(path, false))
                    {
                        writer.WriteLine(JsonUtility.ToJson(saveFile));
                        writer.Close();
                    }
                }
            }
        }
    }
}