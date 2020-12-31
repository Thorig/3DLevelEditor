using System.IO;
using UnityEditor;

public class FileSelect
{
    static public string selectFile()
    {
        return EditorUtility.OpenFilePanel("Assetbundle manifest", "", "xml");
    }
}
