using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

namespace CircuitPuzzle
{
    [CustomEditor(typeof(DeleteIntercept)), CanEditMultipleObjects]
    public class InterceptKeyboardDeleteEditor : Editor
    {
        protected virtual void OnEnable()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        protected virtual void OnDisable()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyGUI;
        }

        /// <summary>
        /// disable the ability to delete GameObjects in Scene view
        /// </summary>
        protected virtual void OnSceneGUI()
        {
            InterceptKeyboardDelete();
        }

        /// <summary>
        /// disable the ability to delete GameObjects in Hierarchy view
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="selectionRect"></param>
        protected virtual void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            InterceptKeyboardDelete();
        }


        /// <summary>
        /// intercept keyboard delete event
        /// </summary>
        private void InterceptKeyboardDelete()
        {
            // Get current event.
            Event e = Event.current;

            // If current event is KeyDown event and KeyCode is delete.
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete)
            {
                // Use event to cancel deletion.
                e.type = EventType.Used;

                // Give warning upon deletion attempt.
                Debug.LogWarning("You're trying to delete elements of the Circuit Puzzle directly from the hierarchy.\n" +
                    "If you wish to delete puzzle pieces, please use the Puzzle Creator in the inspector to do so.");
            }
        }
    }
}