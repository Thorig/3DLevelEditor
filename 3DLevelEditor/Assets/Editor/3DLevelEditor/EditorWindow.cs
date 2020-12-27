using UnityEditor;
using UnityEngine;

namespace LevelEditor3D.Editor
{
    public class Test : EditorWindow
    {
        static public Material material;
        static public Material materialMouseOver;

        static private UIElements.GridParent gridParent;
        static private GameObject currentTile;

        [MenuItem("Window/3DLevelEditorWindow")]
        static void Init()
        {
            material = Resources.Load("Materials/Grid", typeof(Material)) as Material;
            materialMouseOver = Resources.Load("Materials/GridMouseOver", typeof(Material)) as Material;

            currentTile = null;
            gridParent = new UIElements.GridParent();
            Test window = (Test)EditorWindow.GetWindow(typeof(Test));
            SceneView.duringSceneGui += window.OnSceneGUICustom;
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
            else
            {
                handleMousePosition();
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
            gridParent = null;
        }
    }
}