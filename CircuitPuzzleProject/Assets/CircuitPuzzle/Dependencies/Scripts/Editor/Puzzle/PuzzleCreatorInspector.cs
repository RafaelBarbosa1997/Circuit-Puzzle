using UnityEditor;
using UnityEngine;

namespace CircuitPuzzle
{
    /// <summary>
    /// This is a custom editor for the PuzzleCreator script, which displays an interface to create a circuit puzzle in the inspector.
    /// The interface includes a row and column selector, buttons to apply or cancel changes, and a button to clear the board.
    /// It also includes a limiter option, which when enabled will limit the number of pieces that can be placed on the board.
    /// </summary>
    [CustomEditor(typeof(PuzzleCreator))]
    public class PuzzleCreatorInspector : Editor
    {
        #region FIELDS
        private PuzzleCreator targetPuzzleCreator;
        private InspectorAssetsSO inspectorAssets;
        #endregion

        #region SETUP
        /// <summary>
        /// This method is called when an instance of PuzzleCreator is selected in the inspector.
        /// </summary>
        private void OnEnable()
        {
            // Get selected PuzzleCreator script.
            targetPuzzleCreator = (PuzzleCreator)target;
            if (targetPuzzleCreator == null)
            {
                return;
            }

            // Get reference to the inspector assets for custom editor.
            inspectorAssets = targetPuzzleCreator.GetComponent<PuzzleAssetsHolder>().InspectorAssets;
        }
        #endregion

        #region PRIVATE METHODS
        /// <summary>
        /// This method displays the left and right arrows for the user to select the number of rows or columns.
        /// Either the selected number of rows or columns will be sent in as a parameter.
        /// That value is changed when the arrow buttons are clicked, and then returned to change values in the inspector.
        /// </summary>
        /// <param name="axisSelectedValue"></param>
        /// <returns></returns>
        private int DisplaySizeSelectorArrows(int axisSelectedValue)
        {
            EditorGUILayout.BeginHorizontal();

            // Left Arrow.
            if (GUILayout.Button(inspectorAssets.LeftArrow))
            {
                axisSelectedValue--;
            }

            // Right Arrow.
            if (GUILayout.Button(inspectorAssets.RightArrow))
            {
                axisSelectedValue++;
            }

            EditorGUILayout.EndHorizontal();

            return axisSelectedValue;
        }

        /// <summary>
        /// This method compares the selected value of the rows or columns to the set value.
        /// Returns the GUIStyle to be applies to the IntField according to whether the values are the same or not.
        /// </summary>
        /// <param name="fieldDesignation"></param>
        /// <param name="axisSelectedValue"></param>
        /// <param name="axisSetValue"></param>
        /// <returns></returns>
        private GUIStyle SetFieldStyle(int axisSelectedValue, int axisSetValue)
        {
            // If changes to the number of rows were made.
            if (axisSelectedValue != axisSetValue)
            {
                // Make field red to show that changes have been made.
                return inspectorAssets.ChangesPendingField;
            }

            //If no changes were made.
            else
            {
                // Make field default color to show that no changes were made.
                return inspectorAssets.DefaultField;
            }
        }
        #endregion

        #region GUI
        public override void OnInspectorGUI()
        {
            // Board size header.
            EditorGUILayout.LabelField("Board Size", inspectorAssets.DefaultHeader);

            // Spacing //
            GUILayout.Space(inspectorAssets.ContentSpacing);

            #region PUZZLE SIZE SELECTOR

            #region ROWS SECTION
            EditorGUILayout.BeginHorizontal();

            // Rows label.
            EditorGUILayout.LabelField("Rows", inspectorAssets.DefaultLabel);

            // Displays the IntField for the number of rows.
            // The selected rows value is set according to what the user inputs into the IntField.
            // The GUIStyle of the IntField is set according to whether the selected value is different from the puzzle's set value.
            // This allows to user to know if they have unset changes made in the inspector.
            targetPuzzleCreator.SelectedRows = EditorGUILayout.IntField(targetPuzzleCreator.SelectedRows, SetFieldStyle(targetPuzzleCreator.SelectedRows, targetPuzzleCreator.SetRows));

            EditorGUILayout.EndHorizontal();

            // Displays the left and right arrows for the user to select the number of rows.
            // The selected rows value is set according to what the user clicks.
            targetPuzzleCreator.SelectedRows = DisplaySizeSelectorArrows(targetPuzzleCreator.SelectedRows);
            #endregion

            // Spacing //
            GUILayout.Space(inspectorAssets.ContentSpacing);

            #region COLUMNS SECTION
            EditorGUILayout.BeginHorizontal();

            // Columns label.
            EditorGUILayout.LabelField("Columns", inspectorAssets.DefaultLabel);

            // Displays the IntField for the number of columns.
            // Check rows section for full explanation.
            targetPuzzleCreator.SelectedColumns = EditorGUILayout.IntField(targetPuzzleCreator.SelectedColumns, SetFieldStyle(targetPuzzleCreator.SelectedColumns, targetPuzzleCreator.SetColumns));

            EditorGUILayout.EndHorizontal();

            // Displays the left and right arrows for the user to select the number of columns.
            // The selected columns value is set according to what the user clicks.
            targetPuzzleCreator.SelectedColumns = DisplaySizeSelectorArrows(targetPuzzleCreator.SelectedColumns);
            #endregion

            #endregion

            // Spacing //
            GUILayout.Space(inspectorAssets.ContentSpacing);

            #region APPLY AND CANCEL BUTTONS
            // APPLY AND CANCEL SECTION.
            // Change styles for the buttons depending on whether changes were made to row or column count.
            GUIStyle currentApply = new GUIStyle();
            GUIStyle currentCancel = new GUIStyle();
            // If changes were made.
            if(targetPuzzleCreator.SelectedColumns != targetPuzzleCreator.SetColumns || targetPuzzleCreator.SelectedRows != targetPuzzleCreator.SetRows)
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
                    targetPuzzleCreator.ApplyChanges();
                }

                // Spacing //
                GUILayout.Space(inspectorAssets.GroupSpacing);

                // Cancel button.
                if (GUILayout.Button("Cancel", currentCancel))
                {
                    targetPuzzleCreator.CancelChanges();
                }
            EditorGUILayout.EndVertical();
            #endregion

            // Spacing //
            GUILayout.Space(inspectorAssets.ContentSpacing);

            #region CLEAR BOARD
            // Clear board button.
            if (GUILayout.Button("Clear Board", inspectorAssets.DefaultButton))
            {
                // Create popup to confirm whether user wants to clear the board.
                bool clearOutput = EditorUtility.DisplayDialog("Clear Board", "Are you sure you wanna clear the current board?", "Yes", "No");

                // If user clicked yes, clear the board.
                if(clearOutput)
                {
                    targetPuzzleCreator.ClearBoard();
                }
            }
            #endregion

            // Spacing //
            GUILayout.Space(inspectorAssets.ContentSpacing);

            #region LIMITER
            // LIMITER SECTION.
            // Title.
            GUILayout.Label("Limiter", inspectorAssets.DefaultLabel);

            // Spacing //
            GUILayout.Space(inspectorAssets.GroupSpacing);

            // Set button styles.
            GUIStyle enabledStyle;
            GUIStyle disabledStyle;

            if (targetPuzzleCreator.IsLimited)
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
                targetPuzzleCreator.IsLimited = true;
            }

            // Spacing //
            GUILayout.Space(inspectorAssets.GroupSpacing);

            // Disabled button.
            if (GUILayout.Button("Disabled", disabledStyle))
            {
                targetPuzzleCreator.IsLimited = false;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            #endregion

            #region Preview
            // Preview Section.
            if ((targetPuzzleCreator.PreviewRows != targetPuzzleCreator.SelectedRows || targetPuzzleCreator.PreviewColumns != targetPuzzleCreator.SelectedColumns))
            {
                targetPuzzleCreator.GeneratePreview();
            }

            else if ((targetPuzzleCreator.PreviewPieces.GetLength(0) > 0 && targetPuzzleCreator.PreviewPieces.GetLength(1) > 0) && (targetPuzzleCreator.SelectedColumns == targetPuzzleCreator.SetColumns && targetPuzzleCreator.SelectedRows == targetPuzzleCreator.SetRows))
            {
                targetPuzzleCreator.ResetPreview();
            }
            #endregion

            // Repaint so button hover states are reflected in real time.
            Repaint();
        }
    }
    #endregion
}