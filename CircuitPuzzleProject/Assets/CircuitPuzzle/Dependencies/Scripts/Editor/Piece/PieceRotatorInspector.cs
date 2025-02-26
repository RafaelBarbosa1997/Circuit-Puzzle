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
        private float spacing = 10;
        #endregion

        #region GUI
        public override void OnInspectorGUI()
        {
            // Get reference to rotator script.
            PieceRotator rotator = (PieceRotator)target;

            // Header.
            GUILayout.Label("Piece Rotation", rotator.References.PuzzleCreatorAssets.HeaderStyle);

            // Spacing //
            GUILayout.Space(spacing);

            // Buttons.
            GUILayout.BeginHorizontal();
                // Left button.
                if (GUILayout.Button(rotator.References.PuzzleCreatorAssets.LeftArrow))
                {
                    rotator.RotateLeft();
                }

                // Right button.
                if (GUILayout.Button(rotator.References.PuzzleCreatorAssets.RightArrow))
                {
                    rotator.RotateRight();
                }
            GUILayout.EndHorizontal();
        }
        #endregion
    }
}