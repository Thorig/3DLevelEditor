using UnityEngine;
using UnityEditor;

namespace LevelEditor3D.Editor.UIElements
{
    public class GridParent
    {
        private GameObject gridParent;
        private Vector3 movement;
        private bool lctrlPressed = false;

        public GridParent()
        {
            movement = new Vector3();
            gridParent = GameObject.FindGameObjectWithTag("GridParent");
        }

        public void handleKeyStrokes()
        {
            if (Event.current.keyCode == KeyCode.LeftControl)
            {
                setLctrlPressed(true);
            }
            else
            {
                updatePosition(Event.current.keyCode);
            }
        }

        public void setLctrlPressed(bool isPressed)
        {
            lctrlPressed = isPressed;
        }

        public void updatePosition(KeyCode keyCode)
        {
            movement.x = 0;
            movement.y = 0;
            movement.z = 0;
            if (keyCode == KeyCode.K)
            {
                movement.z = -1;
            }
            if (keyCode == KeyCode.I)
            {
                movement.z = 1;
            }
            if (keyCode == KeyCode.L)
            {
                movement.x = 1;
            }
            if (keyCode == KeyCode.J)
            {
                movement.x = -1;
            }
            if (keyCode == KeyCode.U)
            {
                movement.y = -1;
            }
            if (keyCode == KeyCode.O)
            {
                movement.y = 1;
            }
            gridParent.transform.position += movement;
            EditorUtility.SetDirty(gridParent);
        }
    }
}