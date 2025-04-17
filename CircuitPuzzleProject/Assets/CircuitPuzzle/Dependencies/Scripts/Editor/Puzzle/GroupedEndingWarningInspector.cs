using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CircuitPuzzle
{
    [CustomEditor(typeof(GroupedEndingWarning))]
    public class GroupedEndingWarningInspector : Editor
    {
        #region FIELDS
        private const float spacing = 10;
        #endregion

        public override void OnInspectorGUI()
        {
            // Get target.
            GroupedEndingWarning warning = (GroupedEndingWarning)target;
            if (warning == null)
            {
                return;
            }

            // Get reference to the inspector assets for custom editor.
            InspectorAssetsSO inspectorAssets = warning.GetComponent<SOAssetHolder>().InspectorAssets;

            // Check if puzzle is in grouped or single mode.
            bool active = false;

            if (warning.PuzzleSettings.IsGrouped) active = true;

            // Display message according to puzzle mode.
            string warningMessage = "";
            string messageDetails = "";
            string functionMessage = "";

            GUIStyle headerColor = inspectorAssets.GroupedWarningHeader;

            // If in grouped mode.
            if(active == true)
            {
                warningMessage = "Grouped mode is enabled";
                messageDetails = "Use events below to setup behavior for turning power on and off.";
                functionMessage = "";

                headerColor.normal.textColor = Color.green;
            }

            // If in single mode.
            else
            {
                warningMessage = "Grouped mode is disabled";
                messageDetails = "Use events on each individual ending piece or turn on grouped mode.";
                functionMessage = "Events added below will NOT function";

                headerColor.normal.textColor = Color.red;
            }

            // Display message.
            GUILayout.BeginVertical();

            GUILayout.Label(warningMessage, headerColor);

            GUILayout.Space(spacing);

            GUILayout.Label(messageDetails, inspectorAssets.GroupedWarningLabel);

            GUILayout.Space(spacing);

            GUILayout.Label(functionMessage, inspectorAssets.GroupedWarningSubLabel);

            GUILayout.EndVertical();
        }
    }
}
