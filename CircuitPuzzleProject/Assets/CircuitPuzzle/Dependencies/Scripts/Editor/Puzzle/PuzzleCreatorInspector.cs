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
        private bool changesMade;

        private GUIStyle applyStyle;
        private GUIStyle cancelStyle;
        private GUIStyle limiterEnabledStyle;
        private GUIStyle limiterDisabledStyle;

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
        /// This method compares the selected value of the rows or columns to the set value.
        /// Returns the GUIStyle to be applied to the IntField according to whether the values are the same or not.
        /// Also sets bool that indicates whether changes were made to the puzzle, and will determine the GUIStyle of the cancel and apply button accordingly.
        /// </summary>
        /// <param name="fieldDesignation"></param>
        /// <param name="axisSelectedValue"></param>
        /// <param name="axisSetValue"></param>
        /// <returns></returns>
        private GUIStyle SetupStylesForChanges(int axisSelectedValue, int axisSetValue)
        {
            // If changes to the number of rows were made.
            if (axisSelectedValue != axisSetValue)
            {
                // Set changesMade to true, so cancel and apply buttons have styles indicating changes can be applied or cancelled.
                changesMade = true;

                // return GUIStyle with red field, to show changes were made.
                return inspectorAssets.ChangesPendingField;
            }

            //If no changes were made.
            else
            {
                // Return default field GUIStyle, indicating no changes were made.
                return inspectorAssets.DefaultField;
            }
        }

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
        /// This method sets the GUIStyle for the apply and cancel buttons.
        /// If changes were made to the puzzle, the apply button will be green and the cancel button will be red, to indicate to the user they can apply or cancel their changes.
        /// Otherwise, the buttons will have the default style.
        /// </summary>
        private void SetApplyCancelStyles()
        {
            // if changes were made.
            if (changesMade)
            {
                applyStyle = inspectorAssets.GreenButton;
                cancelStyle = inspectorAssets.RedButton;
            }

            // if no changes were made.
            else
            {
                applyStyle = inspectorAssets.DefaultButton;
                cancelStyle = inspectorAssets.DefaultButton;
            }
        }

        private void SetLimiterStyles()
        {
            if (targetPuzzleCreator.IsLimited)
            {
                limiterEnabledStyle = inspectorAssets.ActiveButton;
                limiterDisabledStyle = inspectorAssets.InactiveButton;
            }

            else
            {
                limiterEnabledStyle = inspectorAssets.InactiveButton;
                limiterDisabledStyle = inspectorAssets.ActiveButton;
            }
        }
        #endregion

        #region GUI
        public override void OnInspectorGUI()
        {
            // Initialize bool that determines whether changes to the puzzle were made.
            // Starts as false, is set to true in the size selector section, if the user changes the number of rows or columns.
            changesMade = false;

            // Board size header.
            EditorGUILayout.LabelField("Board Size", inspectorAssets.DefaultHeader);

            // Spacing //
            GUILayout.Space(inspectorAssets.ContentSpacing);

            #region PUZZLE SIZE SELECTOR

            #region ROWS SECTION
            EditorGUILayout.BeginHorizontal();

            // Rows label.
            EditorGUILayout.LabelField("Rows", inspectorAssets.DefaultLabel);

            // Checks if the user has changed the number of rows in the IntField.
            EditorGUI.BeginChangeCheck();

            // Displays the IntField for the number of rows.
            // The selected rows value is set according to what the user inputs into the IntField.
            // The GUIStyle of the IntField is set according to whether the selected value is different from the puzzle's set value.
            // This allows to user to know if they have unset changes made in the inspector.
            targetPuzzleCreator.SelectedRows = EditorGUILayout.IntField(targetPuzzleCreator.SelectedRows, SetupStylesForChanges(targetPuzzleCreator.SelectedRows, targetPuzzleCreator.SetRows));

            EditorGUILayout.EndHorizontal();

            // Displays the left and right arrows for the user to select the number of rows.
            // The selected rows value is set according to what the user clicks.
            targetPuzzleCreator.SelectedRows = DisplaySizeSelectorArrows(targetPuzzleCreator.SelectedRows);

            // If the user has changed the number of rows, check if the preview needs to be updated.
            if (EditorGUI.EndChangeCheck())
            {
                targetPuzzleCreator.DeterminePreviewAdjustments();
            }
            #endregion

            // Spacing //
            GUILayout.Space(inspectorAssets.ContentSpacing);

            #region COLUMNS SECTION
            EditorGUILayout.BeginHorizontal();

            // Columns label.
            EditorGUILayout.LabelField("Columns", inspectorAssets.DefaultLabel);

            // Checks if the user has changed the number of columns in the IntField.
            EditorGUI.BeginChangeCheck();

            // Displays the IntField for the number of columns.
            // Check rows section for full explanation.
            targetPuzzleCreator.SelectedColumns = EditorGUILayout.IntField(targetPuzzleCreator.SelectedColumns, SetupStylesForChanges(targetPuzzleCreator.SelectedColumns, targetPuzzleCreator.SetColumns));

            EditorGUILayout.EndHorizontal();

            // Displays the left and right arrows for the user to select the number of columns.
            // The selected columns value is set according to what the user clicks.
            targetPuzzleCreator.SelectedColumns = DisplaySizeSelectorArrows(targetPuzzleCreator.SelectedColumns);

            // If the user has changed the number of columns, check if the preview needs to be updated.
            if (EditorGUI.EndChangeCheck())
            {
                targetPuzzleCreator.DeterminePreviewAdjustments();
            }
            #endregion

            #endregion

            // Spacing //
            GUILayout.Space(inspectorAssets.ContentSpacing);

            #region APPLY AND CANCEL BUTTONS
            // Change the GUIStyle of the apply and cancel buttons, depending on whether changes were made to the puzzle.
            // Assigns the correct GUIStyles.
            SetApplyCancelStyles();

            EditorGUILayout.BeginVertical();

            // Display apply button.
            if (GUILayout.Button("Apply", applyStyle))
            {
                // Applies changes to the puzzle when button is clicked.
                targetPuzzleCreator.ApplyChanges();
            }

            // Spacing //
            GUILayout.Space(inspectorAssets.GroupSpacing);

            // Display cancel button.
            if (GUILayout.Button("Cancel", cancelStyle))
            {
                // Cancels changes to the puzzle when button is clicked.
                targetPuzzleCreator.CancelChanges();
            }

            EditorGUILayout.EndVertical();
            #endregion

            // Spacing //
            GUILayout.Space(inspectorAssets.ContentSpacing);

            #region CLEAR BOARD
            if (GUILayout.Button("Clear Board", inspectorAssets.DefaultButton))
            {
                // Create popup to confirm whether user wants to clear the board.
                bool clearOutput = EditorUtility.DisplayDialog("Clear Board", "Are you sure you want to clear the current board?", "Yes", "No");

                // If user clicked yes, clear the board.
                if (clearOutput)
                {
                    targetPuzzleCreator.ClearBoard();
                }
            }
            #endregion

            // Spacing //
            GUILayout.Space(inspectorAssets.ContentSpacing);

            #region LIMITER
            // Limiter label.
            GUILayout.Label("Limiter", inspectorAssets.DefaultLabel);

            // Spacing //
            GUILayout.Space(inspectorAssets.GroupSpacing);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Set the GUIStyle of the buttons according to whether the limiter is enabled or not.
            SetLimiterStyles();

            // Enabled button.
            if (GUILayout.Button("Enabled", limiterEnabledStyle))
            {
                targetPuzzleCreator.IsLimited = true;
            }

            // Spacing //
            GUILayout.Space(inspectorAssets.GroupSpacing);

            // Disabled button.
            if (GUILayout.Button("Disabled", limiterDisabledStyle))
            {
                targetPuzzleCreator.IsLimited = false;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            #endregion

            // Repaint so button hover states are reflected in real time.
            Repaint();
        }
    }
    #endregion
}