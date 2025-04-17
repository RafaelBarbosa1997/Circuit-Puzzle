using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CircuitPuzzle
{
    [CustomEditor(typeof(PieceRotator))]
    public class PieceRotatorInspector : Editor
    {
        #region FIELDS
        // Spacing value.
        private const float spacing = 10;
        #endregion

        #region GUI
        public override void OnInspectorGUI()
        {
            // Get reference to rotator script.
            PieceRotator rotator = (PieceRotator)target;
            if(rotator == null)
            {
                return;
            }

            // Get reference to the inspector assets for custom editor.
            InspectorAssetsSO inspectorAssets = rotator.GetComponent<SOAssetHolder>().InspectorAssets;

            // Header.
            GUILayout.Label("Piece Rotation", inspectorAssets.DefaultHeader);

            // Spacing //
            GUILayout.Space(spacing);

            // Buttons.
            GUILayout.BeginHorizontal();
                // Left button.
                if (GUILayout.Button(inspectorAssets.LeftArrow))
                {
                    rotator.RotateLeft();
                }

                // Right button.
                if (GUILayout.Button(inspectorAssets.RightArrow))
                {
                    rotator.RotateRight();
                }
            GUILayout.EndHorizontal();
        }
        #endregion
    }
}