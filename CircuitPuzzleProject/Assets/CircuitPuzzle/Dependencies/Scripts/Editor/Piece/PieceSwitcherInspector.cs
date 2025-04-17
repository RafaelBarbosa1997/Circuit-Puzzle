using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PlasticPipe.PlasticProtocol.Messages;

namespace CircuitPuzzle
{
    [CustomEditor(typeof(PieceSwitcher))]
    public class PieceSwitcherInspector : Editor
    {
        #region FIELDS
        // Button styles.
        private GUIStyle[] buttons;

        // Spacing.
        private const float spacing = 10;
        #endregion

        #region GUI
        public override void OnInspectorGUI()
        {
            // Get PieceSwitcher reference.
            PieceSwitcher switcher = (PieceSwitcher)target;
            if (switcher == null)
            {
                return;
            }

            // Get reference to the inspector assets for custom editor.
            InspectorAssetsSO inspectorAssets = switcher.GetComponent<SOAssetHolder>().InspectorAssets;

            // Set the active style on active button.
            SetActiveButton(switcher, inspectorAssets);

            GUILayout.BeginVertical();
                // Title.
                GUILayout.Label("Piece Switcher", inspectorAssets.DefaultHeader);

                // Spacing //
                GUILayout.Space(spacing * 2);

                // Start piece button.
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Start", buttons[4]))
                {
                    switcher.SwitchPiece(switcher.References.PieceAssets.StartPiece, 4);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                // Spacing //
                GUILayout.Space(spacing);

                // End piece button.
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("End", buttons[5]))
                {
                    switcher.SwitchPiece(switcher.References.PieceAssets.EndPiece, 5);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                // Spacing //
                GUILayout.Space(spacing);

                // Straight piece button.
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Straight", buttons[1]))
                {
                    switcher.SwitchPiece(switcher.References.PieceAssets.StraightPiece, 1);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                // Spacing //
                GUILayout.Space(spacing);

                // Corner piece button.
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Corner", buttons[3]))
                {
                    switcher.SwitchPiece(switcher.References.PieceAssets.CornerPiece, 3);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                // Spacing //
                GUILayout.Space(spacing);

                // T piece button.
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("T", buttons[2]))
                {
                    switcher.SwitchPiece(switcher.References.PieceAssets.TPiece, 2);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                // Spacing //
                GUILayout.Space(spacing);

                // Blank Piece Button.
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Blank", buttons[0]))
                {
                    switcher.SwitchPiece(switcher.References.PieceAssets.BlankPiece, 0);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            // Repaint so button hover changes are reflected.
            Repaint();
        }
        #endregion

        #region PRIVATE METHODS
        /// <summary>
        ///  Sets the the active button according to piece type.
        /// </summary>
        private void SetActiveButton(PieceSwitcher switcher, InspectorAssetsSO inspectorAssets)
        {
            // Create button array if it is null.
            if(buttons == null)
            {
                buttons = new GUIStyle[6];
            }

            // Get the type index.
            PieceBase baseSelect = switcher.GetComponent<PieceBase>();
            int typeIndex = baseSelect.TypeIndex;

            // Set active button to active style and all others to inactive style.
            for(int i = 0; i < buttons.Length; i++)
            {
                if(i == typeIndex)
                {
                    buttons[i] = inspectorAssets.ActiveButton;
                }

                else
                {
                    buttons[i] = inspectorAssets.InactiveButton;
                }
            }
        }
        #endregion
    }
}