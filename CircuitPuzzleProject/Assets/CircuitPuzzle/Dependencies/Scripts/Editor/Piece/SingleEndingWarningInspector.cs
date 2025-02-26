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
        private float spacing = 10;
        #endregion

        public override void OnInspectorGUI()
        {
            // Get script reference.
            SingleEndingWarning warning = (SingleEndingWarning)target;
            if (warning == null)
            {
                return;
            }

            // Check if puzzle is in grouped or single mode.
            bool active = false;

            if (warning.PuzzleSettings.IsGrouped == false) active = true;

            // Display message according to puzzle mode.
            string warningMessage = "";
            string messageDetails = "";
            string functionMessage = "";

            GUIStyle headerColor = warning.Assets.PuzzleCreatorAssets.WarningHeaderStyle;

            // If in grouped mode.
            if (active == true)
            {
                warningMessage = "Single mode is enabled";
                messageDetails = "Use events below to setup behavior for turning power on and off for this specific piece.";
                functionMessage = "";

                headerColor.normal.textColor = Color.green;
            }

            // If in single mode.
            else
            {
                warningMessage = "Single mode is disabled";
                messageDetails = "Use events on main puzzle component or turn on single mode.";
                functionMessage = "Events added below will NOT function";

                headerColor.normal.textColor = Color.red;
            }

            // Display message.
            GUILayout.BeginVertical();

            GUILayout.Label(warningMessage, headerColor);

            GUILayout.Space(spacing);

            GUILayout.Label(messageDetails, warning.Assets.PuzzleCreatorAssets.WarningLabelStyle);

            GUILayout.Space(spacing);

            GUILayout.Label(functionMessage, warning.Assets.PuzzleCreatorAssets.WarningFunctionStyle);

            GUILayout.EndVertical();
        }
    }
}
