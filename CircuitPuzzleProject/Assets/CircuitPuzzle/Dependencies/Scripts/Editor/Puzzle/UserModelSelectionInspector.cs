using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CircuitPuzzle
{
    [CustomEditor(typeof(UserModelSelection))]
    public class UserModelSelectionInspector : Editor
    {
        #region FIELDS
        private const float contentSpacing = 3.5f;
        private const float textSpacing = 1.5f;
        private const float buttonSpacing = 10f;
        #endregion

        public override void OnInspectorGUI()
        {
            // Get target.
            UserModelSelection selection = (UserModelSelection)target;
            if (selection == null)
            {
                return;
            }

            // Get reference to the inspector assets for custom editor.
            InspectorAssetsSO inspectorAssets = selection.GetComponent<PuzzleAssetsHolder>().InspectorAssets;

            // Title.
            GUILayout.Label("Custom Models", inspectorAssets.DefaultHeader);

            GUILayout.Space(contentSpacing);

            // Description.
            GUILayout.Label("Use this toggle to enable your own custom models for pieces.", inspectorAssets.UserModelLabel);

            GUILayout.Space(textSpacing);

            GUILayout.Label("Insert prefabs with models for each desired piece below and then enable. Make sure you've assigned a model for every piece before enabling or it will not work.", inspectorAssets.UserModelLabel);

            GUILayout.Space(textSpacing);

            GUILayout.Label("Piece X and Y size scale must be 1 and 1 in Unity size units.", inspectorAssets.UserModelLabel);

            GUILayout.Space(contentSpacing * 2);

            // Button section.
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Get correct button style.
            GUIStyle enabledButton;

            if (selection.UserModelsEnabled) enabledButton = inspectorAssets.GreenButton;
            else enabledButton = inspectorAssets.DefaultButton;

            // Enabled button.
            if(GUILayout.Button("Enabled", enabledButton))
            {
                selection.EnableCustomModels();
            }

            GUILayout.Space(buttonSpacing);

            // Get correct button style.
            GUIStyle disabledButton;

            if (selection.UserModelsEnabled == false) disabledButton = inspectorAssets.RedButton;
            else disabledButton = inspectorAssets.DefaultButton;

            // Disabled button.
            if(GUILayout.Button("Disabled", disabledButton))
            {
                selection.DisableCustomModels();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}
