using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace CircuitPuzzle
{
    [CustomEditor(typeof(PuzzleCreator))]
    public class PuzzleCreatorInspector : Editor
    {
        #region FIELDS
        // Spacing.
        private const int contentSpacing = 20;
        private const int groupSpacing = 5;
        #endregion

        #region GUI
        public override void OnInspectorGUI()
        {
            #region SETUP
            // Get selected PuzzleCreator script.
            // Returns if casting fails.
            PuzzleCreator creator = (PuzzleCreator)target;
            if (creator == null)
            {
                return;
            }

            // Get reference to the inspector assets for custom editor.
            InspectorAssetsSO inspectorAssets = creator.GetComponent<SOAssetHolder>().InspectorAssets;
            #endregion

            // Bool to setup creation preview.
            bool preview = true;

            //// Initial undo warning.
            //if (creator.UndoCleared == false || PrefabUtility.IsPartOfAnyPrefab(creator.gameObject))
            //{
            //    bool instantiate = EditorUtility.DisplayDialog("Circuit Puzzle", "To avoid errors, creating a circuit puzzle instance clears the undo history.\n" +
            //        "If you wish to revert any changes in the scene, do so before creating a circuit puzzle.", "Continue", "Cancel");

            //    if (instantiate)
            //    {
            //        Undo.ClearAll();
            //    }

            //    else
            //    {
            //        preview = false;
            //        DestroyImmediate(creator.gameObject);
            //    }

            //    PrefabUtility.UnpackPrefabInstance(creator.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

            //    creator.UndoCleared = true;
            //}

            #region LAYOUT
            // Board size settings header.
            EditorGUILayout.LabelField("Board Size", inspectorAssets.DefaultHeader);

            // Spacing //
            GUILayout.Space(contentSpacing);

            // ROWS SECTION.
            // Field.
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Rows", inspectorAssets.DefaultLabel);
                // If changes to the number of rows were made.
                if(creator.SelectedRows != creator.SetRows)
                {
                    // Make field red to show that changes have been made.
                    creator.SelectedRows = EditorGUILayout.IntField(creator.SelectedRows, inspectorAssets.ChangesPendingField);
                }

                 //If no changes were made.
                else
                {
                    // Make field default color to show that no changes were made.
                    creator.SelectedRows = EditorGUILayout.IntField(creator.SelectedRows, inspectorAssets.DefaultField);
                }
            EditorGUILayout.EndHorizontal();

            // Spacing //
            GUILayout.Space(groupSpacing);

            // Arrow Buttons.
            EditorGUILayout.BeginHorizontal();
                // Left Arrow.
                if (GUILayout.Button(inspectorAssets.LeftArrow))
                {
                    creator.SelectedRows--;
                }

                // Right Arrow.
                if (GUILayout.Button(inspectorAssets.RightArrow))
                {
                    creator.SelectedRows++;
                }
            EditorGUILayout.EndHorizontal();

            // Spacing //
            GUILayout.Space(contentSpacing);

            // COLUMNS SECTION.
            // Field.
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Columns", inspectorAssets.DefaultLabel);
            // If changes to the number of columns have been made.
            if (creator.SetColumns != creator.SelectedColumns)
            {
                // Make field red to show that changes have been made.
                creator.SelectedColumns = EditorGUILayout.IntField(creator.SelectedColumns, inspectorAssets.ChangesPendingField);
            }

            // If no changes were made.
            else
            {
                // Make field default color to show that no changes have been made.
                creator.SelectedColumns = EditorGUILayout.IntField(creator.SelectedColumns, inspectorAssets.DefaultField);
            }
            EditorGUILayout.EndHorizontal();

            // Spacing //
            GUILayout.Space(groupSpacing);

            // Arrow Buttons.
            EditorGUILayout.BeginHorizontal();
            // Left Arrow.
            if (GUILayout.Button(inspectorAssets.LeftArrow))
            {
                creator.SelectedColumns--;
            }

            // Right Arrow.
            if (GUILayout.Button(inspectorAssets.RightArrow))
            {
                creator.SelectedColumns++;
            }
            EditorGUILayout.EndHorizontal();            

            // Spacing //
            GUILayout.Space(contentSpacing);

            // APPLY AND CANCEL SECTION.
            // Change styles for the buttons depending on whether changes were made to row or column count.
            GUIStyle currentApply = new GUIStyle();
            GUIStyle currentCancel = new GUIStyle();
            // If changes were made.
            if(creator.SelectedColumns != creator.SetColumns || creator.SelectedRows != creator.SetRows)
            {
                currentApply = inspectorAssets.GreenButton;
                currentCancel = inspectorAssets.RedButton;
            }

            // If changes were not made.
            else
            {
                currentApply = inspectorAssets.DefaultButton;
                currentCancel = inspectorAssets.DefaultButton;
            }
            EditorGUILayout.BeginVertical();
                // Apply button.
                if (GUILayout.Button("Apply", currentApply))
                {
                    creator.ApplyChanges();
                }

                // Spacing //
                GUILayout.Space(groupSpacing);

                // Cancel button.
                if (GUILayout.Button("Cancel", currentCancel))
                {
                    creator.CancelChanges();
                }
            EditorGUILayout.EndVertical();

            // Spacing //
            GUILayout.Space(contentSpacing);

            // Clear board button.
            if(GUILayout.Button("Clear Board", inspectorAssets.DefaultButton))
            {
                // Create popup to confirm whether user wants to clear the board.
                bool clearOutput = EditorUtility.DisplayDialog("Clear Board", "Are you sure you wanna clear the current board?", "Yes", "No");

                // If user clicked yes, clear the board.
                if(clearOutput)
                {
                    creator.ClearBoard();
                }
            }

            // Preview Section.
            if((creator.PreviewRows != creator.SelectedRows || creator.PreviewColumns != creator.SelectedColumns) && preview == true)
            {
                creator.GeneratePreview();
            }

            else if((creator.PreviewPieces.GetLength(0) > 0 && creator.PreviewPieces.GetLength(1) > 0) && (creator.SelectedColumns == creator.SetColumns && creator.SelectedRows == creator.SetRows))
            {
                creator.ResetPreview();
            }

            // Spacing //
            GUILayout.Space(contentSpacing);

            // LIMITER SECTION.
            // Title.
            GUILayout.Label("Limiter", inspectorAssets.DefaultLabel);

            // Spacing //
            GUILayout.Space(groupSpacing * 2);

            // Set button styles.
            GUIStyle enabledStyle;
            GUIStyle disabledStyle;

            if (creator.IsLimited)
            {
                enabledStyle = inspectorAssets.ActiveButton;
                disabledStyle = inspectorAssets.InactiveButton;
            }

            else
            {
                enabledStyle = inspectorAssets.InactiveButton;
                disabledStyle = inspectorAssets.ActiveButton;
            }

            // Buttons.
            // Enabled button.
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Enabled button.
            if (GUILayout.Button("Enabled", enabledStyle))
            {
                creator.IsLimited = true;
            }

            // Spacing //
            GUILayout.Space(groupSpacing * 2);

            // Disabled button.
            if (GUILayout.Button("Disabled", disabledStyle))
            {
                creator.IsLimited = false;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // Repaint so button hover states are reflected in real time.
            Repaint();
        }
        #endregion
    }
    #endregion
}