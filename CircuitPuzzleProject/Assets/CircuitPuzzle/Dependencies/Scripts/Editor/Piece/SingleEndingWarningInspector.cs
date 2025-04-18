using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CircuitPuzzle
{
    [CustomEditor(typeof(SingleEndingWarning))]
    public class SingleEndingWarningInspector : Editor
    {
        #region FIELDS
        private const float spacing = 10;
        #endregion

        public override void OnInspectorGUI()
        {
            // Get target.
            SingleEndingWarning warning = (SingleEndingWarning)target;
            if (warning == null)
            {
                return;
            }

            // Get reference to the inspector assets for custom editor.
            InspectorAssetsSO inspectorAssets = warning.GetComponent<SOAssetHolder>().InspectorAssets;

            // Initialize variables to be used in warning message.
            string warningMessage = "";
            string messageDetails = "";
            string functionMessage = "";

            GUIStyle headerColor = inspectorAssets.GroupedWarningHeader;

            // When piece isn't part of a puzzle (most likely prefab mode).
            if (warning.PuzzleSettings == null)
            {
                warningMessage = "Piece is not part of a puzzle";
                messageDetails = "This piece is not part of a puzzle, so it cannot be grouped or single.";
                functionMessage = "This probably means you are viewing this piece in prefab mode.";

                headerColor.normal.textColor = Color.white;

                DisplayWarning(warningMessage, messageDetails, functionMessage, headerColor, inspectorAssets);

                return;
            }

            // If puzzle is in single mode.
            if (warning.PuzzleSettings.IsGrouped == false)
            {
                warningMessage = "Single mode is enabled";
                messageDetails = "Use events below to setup behavior for turning power on and off for this specific piece.";
                functionMessage = "";

                headerColor.normal.textColor = Color.green;

                DisplayWarning(warningMessage, messageDetails, functionMessage, headerColor, inspectorAssets);
            }

            // If puzzle is in grouped mode.
            else
            {
                warningMessage = "Single mode is disabled";
                messageDetails = "Use events on main puzzle component or turn on single mode.";
                functionMessage = "Events added below will NOT function";

                headerColor.normal.textColor = Color.red;

                DisplayWarning(warningMessage, messageDetails, functionMessage, headerColor, inspectorAssets);
            }
        }

        /// <summary>
        /// Displays the warning message in the inspector, according to whether puzzle is in single or grouped mode.
        /// </summary>
        /// <param name="warningMessage"></param>
        /// <param name="messageDetails"></param>
        /// <param name="functionMessage"></param>
        /// <param name="warningColor"></param>
        /// <param name="inspectorAssets"></param>
        private void DisplayWarning(string warningMessage, string messageDetails, string functionMessage, GUIStyle warningColor, InspectorAssetsSO inspectorAssets)
        {
            GUILayout.BeginVertical();

            GUILayout.Label(warningMessage, warningColor);

            GUILayout.Space(spacing);

            GUILayout.Label(messageDetails, inspectorAssets.GroupedWarningLabel);

            GUILayout.Space(spacing);

            GUILayout.Label(functionMessage, inspectorAssets.GroupedWarningSubLabel);

            GUILayout.EndVertical();
        }
    }
}
