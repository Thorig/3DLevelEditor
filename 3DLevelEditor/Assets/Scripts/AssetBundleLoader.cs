using UnityEngine;

public class AssetBundleLoader : MonoBehaviour
{
    public string assetName = "Cliff_Solo";
    public string assetMaterialName = "Cliff";
    public string assetTextureName = "Cliff";
    public string bundleName = "E:\\Projects\\AssetBundle\\AssetBundle\\Assets\\StreamingAssets\\assetbundlebasic";

    // Start is called before the first frame update
    void Start() 
    {
        AssetBundle localAssetBundle = AssetBundle.LoadFromFile(bundleName);

        if(localAssetBundle != null)
        {
            Debug.Log(assetMaterialName);
//            Texture2D assetTexture = localAssetBundle.LoadAsset<Texture2D>(assetTextureName);
            Material assetMaterial = localAssetBundle.LoadAsset<Material>(assetMaterialName);
  //          assetMaterial.SetTexture("Albedo", assetTexture);
            GameObject asset = localAssetBundle.LoadAsset<GameObject>(assetName);
         //   asset.GetComponent<MeshRenderer>().material = assetMaterial;
            Instantiate(asset);
            localAssetBundle.Unload(false);
        }
    }
}
