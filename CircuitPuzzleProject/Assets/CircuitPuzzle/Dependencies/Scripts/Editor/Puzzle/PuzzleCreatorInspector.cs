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
        private int contentSpacing = 20;
        private int groupSpacing = 5;

        // Styles.
        private GUIStyle headerStyle;
        private GUIStyle labelStyle;
        private GUIStyle fieldStyle;
        private GUIStyle buttonStyle;
        private GUIStyle fieldChangesStyle;
        private GUIStyle buttonGreenStyle;
        private GUIStyle buttonRedStyle;
        private GUIStyle activeButton;
        private GUIStyle inactiveButton;

        // Textures.
        private Texture2D leftArrowTexture;
        private Texture2D rightArrowTexture;
        #endregion

        #region GUI
        public override void OnInspectorGUI()
        {
            #region SETUP
            // Get selected PuzzleCreator script.
            // Returns if casting fails.
            PuzzleCreator creator = target as PuzzleCreator;
            if (creator == null)
            {
                return;
            }

            // Set styles for UI elements.
            headerStyle = creator.References.PuzzleCreatorAssets.HeaderStyle;
            labelStyle = creator.References.PuzzleCreatorAssets.LabelStyle;
            fieldStyle = creator.References.PuzzleCreatorAssets.FieldStyle;
            buttonStyle= creator.References.PuzzleCreatorAssets.ButtonStyle;
            fieldChangesStyle = creator.References.PuzzleCreatorAssets.FieldChangesStyle;
            buttonGreenStyle = creator.References.PuzzleCreatorAssets.ButtonGreenStyle;
            buttonRedStyle = creator.References.PuzzleCreatorAssets.ButtonRedStyle;
            activeButton = creator.References.PieceAssets.ActiveButton;
            inactiveButton = creator.References.PieceAssets.InactiveButton;

            // Set textures for UI elements.
            leftArrowTexture = creator.References.PuzzleCreatorAssets.LeftArrow;
            rightArrowTexture = creator.References.PuzzleCreatorAssets.RightArrow;
            #endregion

            // Bool to setup creation preview.
            bool preview = true;

            // Initial undo warning.
            if (creator.UndoCleared == false || PrefabUtility.IsPartOfAnyPrefab(creator.gameObject))
            {
                bool instantiate = EditorUtility.DisplayDialog("Circuit Puzzle", "To avoid errors, creating a circuit puzzle instance clears the undo history.\n" +
                    "If you wish to revert any changes in the scene, do so before creating a circuit puzzle.", "Continue", "Cancel");

                if (instantiate)
                {
                    Undo.ClearAll();
                }

                else
                {
                    preview = false;
                    DestroyImmediate(creator.gameObject);
                }

                PrefabUtility.UnpackPrefabInstance(creator.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

                creator.UndoCleared = true;
            }

            #region LAYOUT
            // Board size settings header.
            EditorGUILayout.LabelField("Board Size", headerStyle);

            // Spacing //
            GUILayout.Space(contentSpacing);

            // ROWS SECTION.
            // Field.
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Rows", labelStyle);
                // If changes to the number of rows were made.
                if(creator.SelectedRows != creator.SetRows)
                {
                    // Make field red to show that changes have been made.
                    creator.SelectedRows = EditorGUILayout.IntField(creator.SelectedRows, fieldChangesStyle);
                }

                 //If no changes were made.
                else
                {
                    // Make field default color to show that no changes were made.
                    creator.SelectedRows = EditorGUILayout.IntField(creator.SelectedRows, fieldStyle);
                }
            EditorGUILayout.EndHorizontal();

            // Spacing //
            GUILayout.Space(groupSpacing);

            // Arrow Buttons.
            EditorGUILayout.BeginHorizontal();
                // Left Arrow.
                if (GUILayout.Button(leftArrowTexture))
                {
                    creator.SelectedRows--;
                }

                // Right Arrow.
                if (GUILayout.Button(rightArrowTexture))
                {
                    creator.SelectedRows++;
                }
            EditorGUILayout.EndHorizontal();

            // Spacing //
            GUILayout.Space(contentSpacing);

            // COLUMNS SECTION.
            // Field.
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Columns", labelStyle);
            // If changes to the number of columns have been made.
            if (creator.SetColumns != creator.SelectedColumns)
            {
                // Make field red to show that changes have been made.
                creator.SelectedColumns = EditorGUILayout.IntField(creator.SelectedColumns, fieldChangesStyle);
            }

            // If no changes were made.
            else
            {
                // Make field default color to show that no changes have been made.
                creator.SelectedColumns = EditorGUILayout.IntField(creator.SelectedColumns, fieldStyle);
            }
            EditorGUILayout.EndHorizontal();

            // Spacing //
            GUILayout.Space(groupSpacing);

            // Arrow Buttons.
            EditorGUILayout.BeginHorizontal();
            // Left Arrow.
            if (GUILayout.Button(leftArrowTexture))
            {
                creator.SelectedColumns--;
            }

            // Right Arrow.
            if (GUILayout.Button(rightArrowTexture))
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
                currentApply = buttonGreenStyle;
                currentCancel = buttonRedStyle;
            }

            // If changes were not made.
            else
            {
                currentApply = buttonStyle;
                currentCancel = buttonStyle;
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
            if(GUILayout.Button("Clear Board", buttonStyle))
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
            GUILayout.Label("Limiter", labelStyle);

            // Spacing //
            GUILayout.Space(groupSpacing * 2);

            // Set button styles.
            GUIStyle enabledStyle;
            GUIStyle disabledStyle;

            if (creator.IsLimited)
            {
                enabledStyle = activeButton;
                disabledStyle = inactiveButton;
            }

            else
            {
                enabledStyle = inactiveButton;
                disabledStyle = activeButton;
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