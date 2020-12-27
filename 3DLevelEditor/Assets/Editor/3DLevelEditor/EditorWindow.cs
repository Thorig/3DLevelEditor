using UnityEditor;
using UnityEngine;

namespace LevelEditor3D.Editor
{
    public class EditorWindow3DLevelEditor : EditorWindow
    {
        static public Material material;
        static public Material materialMouseOver;

        static private UIElements.GridParent gridParent;
        static private GameObject currentTile;


        #region Attributes of the tools
        static private GUIStyle stampStyle;
        static private bool isStampPressed;
        #endregion

        #region Attributes for mouse clicks
        static private bool isLeftMouseButtonPressed;
        #endregion

        [MenuItem("Window/3DLevelEditorWindow")]
        public static void Init()
        {
            EditorWindow.GetWindow(typeof(EditorWindow3DLevelEditor));
        }

        private void OnEnable()
        {
            material = Resources.Load("Materials/Grid", typeof(Material)) as Material;
            materialMouseOver = Resources.Load("Materials/GridMouseOver", typeof(Material)) as Material;

            isStampPressed = true;
            setButtonStylesAndContent();

            currentTile = null;
            gridParent = new UIElements.GridParent();

            SceneView.duringSceneGui += OnSceneGUICustom;
        }

        private void setButtonStylesAndContent()
        {
            if (stampStyle == null)
            {
                stampStyle = setGUIStyleForButton("Images/IconStamp", "Images/IconStampActive");
            }
        }

        private GUIStyle setGUIStyleForButton(string normalButton, string activeButton)
        {
            GUIStyle style = new GUIStyle();
            style.active.textColor = Color.white;
            style.active.background = (Texture2D)Resources.Load("Images/IconStampActive");
            style.hover.background = (Texture2D)Resources.Load("Images/IconStampActive");
            style.hover.textColor = Color.white;
            style.normal.background = (Texture2D)Resources.Load("Images/IconStamp");
            style.normal.textColor = Color.white;
            style.fixedHeight = 32;
            style.fixedWidth = 32;
            style.alignment = TextAnchor.LowerCenter;

            return style;
        }

        void OnGUI()
        {
            GUILayout.Label("Tools");
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Stamp", stampStyle))
            {
                isStampPressed = true;
            }
            GUILayout.EndHorizontal();

            GUILayout.Label("Palette");
            GUILayout.BeginHorizontal("box");
            if(isStampPressed)
            {
                loadPaletteForStamp();
            }
            GUILayout.EndHorizontal();
        }

        private void loadPaletteForStamp()
        {
        
        }

        private void placeAsset(Vector3 position)
        {
            string assetName = "Cliff_Solo";
            string bundleName = "E:\\Projects\\AssetBundle\\AssetBundle\\Assets\\StreamingAssets\\assetbundlebasic";

            AssetBundle localAssetBundle = AssetBundle.LoadFromFile(bundleName);

            if (localAssetBundle != null)
            {
                GameObject asset = localAssetBundle.LoadAsset<GameObject>(assetName);
                asset.transform.position = position;
                Instantiate(asset);
                localAssetBundle.Unload(false);
                EditorUtility.SetDirty(asset);
            }
        }


        public void OnSceneGUICustom(SceneView sceneView)
        {
            if (Event.current.isKey && Event.current.type == EventType.KeyDown)
            {
                handleKeyStrokes();
            }
            else if (Event.current.keyCode == KeyCode.LeftControl && Event.current.type == EventType.KeyUp)
            {
                gridParent.setLctrlPressed(false);
            }
            else if (Event.current.isMouse)
            {
                handleMousePosition();
            }
        }

        private void handleMousePosition()
        {
            if (Event.current.button == 0)
            {
                isLeftMouseButtonPressed = true;
            }
            if (Event.current.button != 0 && isLeftMouseButtonPressed)
            {
                isLeftMouseButtonPressed = false;
                placeAsset(currentTile.transform.position);
            }

            Vector2 guiPosition = Event.current.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(guiPosition);
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit))
            {
                Vector3 labelPosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
                Handles.Label(labelPosition, "Grid Position " + hit.collider.transform.position);
                if (hit.collider.CompareTag("GridTile"))
                {
                    if (currentTile != null)
                    {
                        currentTile.GetComponent<MeshRenderer>().material = material;
                        EditorUtility.SetDirty(currentTile);
                    }
                    currentTile = hit.collider.gameObject;
                    currentTile.GetComponent<MeshRenderer>().material = materialMouseOver;
                    EditorUtility.SetDirty(currentTile);
                }
            }
            else
            {
                if (currentTile != null)
                {
                    currentTile.GetComponent<MeshRenderer>().material = material;
                    EditorUtility.SetDirty(currentTile);
                }
            }
        }

        private void handleKeyStrokes()
        {
            if (Event.current.keyCode == KeyCode.LeftControl)
            {
                gridParent.setLctrlPressed(true);
            }
            else
            {
                gridParent.updatePosition(Event.current.keyCode);
            }
        }

        void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUICustom;
            gridParent = null;
        }
    }
}