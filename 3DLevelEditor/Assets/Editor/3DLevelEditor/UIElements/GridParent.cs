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

        public void setLctrlPressed(bool isPressed)
        {
            lctrlPressed = isPressed;
        }

        public void updatePosition(KeyCode keyCode)
        {
            movement.x = 0;
            movement.y = 0;
            if (keyCode == KeyCode.D)
            {
                movement.x = -1;
            }
            if (keyCode == KeyCode.A)
            {
                movement.x = 1;
            }
            if (keyCode == KeyCode.W)
            {
                movement.y = 1;
            }
            if (!lctrlPressed && keyCode == KeyCode.S)
            {
                movement.y = -1;
            }
            gridParent.transform.position += movement;
            EditorUtility.SetDirty(gridParent);
        }
    }
}