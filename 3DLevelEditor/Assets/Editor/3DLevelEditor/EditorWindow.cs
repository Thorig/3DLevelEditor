using LevelEditor3D.Util;
using System;
using System.Collections.Generic;
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

        #region Attributes for palette
        static private PaletteService paletteService;
        static private int selectedAsset = 0;
        static private int selectedAssetBundle = 0;
        static private string manifestData;
        #endregion

        [MenuItem("Window/3DLevelEditorWindow")]
        public static void Init()
        {
            paletteService = new PaletteService();
            EditorWindow.GetWindow(typeof(EditorWindow3DLevelEditor));
        }

        private void OnEnable()
        {
            material = Resources.Load("Editor3D/Materials/Grid", typeof(Material)) as Material;
            materialMouseOver = Resources.Load("Editor3D/Materials/GridMouseOver", typeof(Material)) as Material;

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
                stampStyle = setGUIStyleForButton("Editor3D/Images/IconStamp", "Editor3D/Images/IconStampActive");
            }
        }

        private GUIStyle setGUIStyleForButton(string normalButton, string activeButton)
        {
            return setGUIStyleForButton((Texture2D)Resources.Load(normalButton), (Texture2D)Resources.Load(activeButton));
        }

        private GUIStyle setGUIStyleForButton(Texture2D normalButton, Texture2D activeButton)
        {
            GUIStyle style = new GUIStyle();
            style.active.textColor = Color.white;
            style.active.background = activeButton;
            style.hover.background = activeButton;
            style.hover.textColor = Color.white;
            style.normal.background = normalButton;
            style.normal.textColor = Color.white;
            style.fixedHeight = 32;
            style.fixedWidth = 32;
            style.alignment = TextAnchor.LowerCenter;
            style.padding = new RectOffset(2, 0, 2, 0);
            style.contentOffset = new Vector2(2, 2);
            style.margin = new RectOffset(2, 0, 2, 0);

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
            if (isStampPressed)
            {
                loadPaletteForStamp();
            }
            GUILayout.EndHorizontal();
        }

        private void loadPaletteForStamp()
        {
            if (!paletteService.isLoaded)
            {
                manifestData = FileSelect.selectFile();
                paletteService.loadPalette(manifestData);                
            }

            List<AssetBundle> bundles = paletteService.getAssetBundles();
            int bundleCounter = 0;

            foreach (AssetBundle bundle in bundles)
            {
                foreach(Prefab p in bundle.prefabList)
                {
                    if (GUILayout.Button(p.name, setGUIStyleForButton(p.textNormal, p.textActive)))
                    {
                        selectedAssetBundle = bundleCounter;
                        selectedAsset = p.index;
                    }
                }
                bundleCounter++;
            }
        }

        public void OnSceneGUICustom(SceneView sceneView)
        {
            if (Event.current.isKey && Event.current.type == EventType.KeyDown)
            {
                gridParent.handleKeyStrokes();
            }
            else if (Event.current.keyCode == KeyCode.LeftControl && Event.current.type == EventType.KeyUp)
            {
                gridParent.setLctrlPressed(false);
            }
            else if (Event.current.isMouse)
            {
                handleMousePosition();
            }
            else 
            {
                if (currentTile != null && Event.current.isKey && Event.current.type == EventType.KeyUp &&
                    Event.current.keyCode == KeyCode.P)
                {
                    paletteService.placeAsset(currentTile.transform.position, selectedAssetBundle, selectedAsset);
                }
            }
        }

        private void handleMousePosition()
        {
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
                else
                {
                    if (currentTile != null)
                    {
                        currentTile.GetComponent<MeshRenderer>().material = material;
                        EditorUtility.SetDirty(currentTile);
                        currentTile = null;
                    }
                }
            }
            else
            {
                if (currentTile != null)
                {
                    currentTile.GetComponent<MeshRenderer>().material = material;
                    EditorUtility.SetDirty(currentTile);
                    currentTile = null;
                }
            }
        }
        
        void OnDestroy()
        {
            paletteService.cleanup();
            paletteService = null;
            SceneView.duringSceneGui -= OnSceneGUICustom;
            gridParent = null;
        }
    }
}