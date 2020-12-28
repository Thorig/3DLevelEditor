using UnityEngine;
using System.Xml;

namespace LevelEditor3D.Util
{
    public class ManifestReader
    {
        public void Main()
        {
            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<?xml version='1.0' ?>" +
                "<prefabs>" +
                    "<prefab filename='Cliff_Solo'>" +
                        "<icon normal='CliffIconNormal' active='CliffIconActive' />" +
                    "</prefab>" +
                "</prefabs>");

            XmlNodeList elemList = doc.GetElementsByTagName("prefab");
            for (int i = 0; i < elemList.Count; i++)
            {
                XmlAttributeCollection attrs = elemList[i].Attributes;
                for (int x = 0; x < attrs.Count; x++)
                {
                    Debug.Log(attrs[x].Name + " " + attrs[x].Value);//filename + value
                }
                if(elemList[i].HasChildNodes)
                {
                    XmlNodeList childList = elemList[i].ChildNodes;

                    for (int y = 0; y < childList.Count; y++)
                    {
                        if (childList[y].Name.CompareTo("icon") == 0)
                        {
                            XmlAttributeCollection childAttrs = childList[y].Attributes;
                            for (int z = 0; z < childAttrs.Count; z++)
                            {
                                Debug.Log(childAttrs[z].Name + " " + childAttrs[z].Value);//normal + value active + value
                            }
                        }
                    }
                }
            }
        }
    }
}
