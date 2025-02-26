using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CircuitPuzzle
{
    [CustomEditor(typeof(UserModelSelection))]
    public class UserModelSelectionInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            // Get target.
            UserModelSelection selection = (UserModelSelection)target;

            // Spacing values.
            float contentSpacing = 3.5f;
            float textSpacing = 1.5f;
            float buttonSpacing = 10f;

            // Styles.
            GUIStyle header = selection.Assets.PuzzleCreatorAssets.HeaderStyle;
            GUIStyle body = selection.Assets.PuzzleCreatorAssets.LabelStyle;
            GUIStyle defaultButton = selection.Assets.PuzzleCreatorAssets.ButtonStyle;
            GUIStyle greenButton = selection.Assets.PuzzleCreatorAssets.ButtonGreenStyle;
            GUIStyle redButton = selection.Assets.PuzzleCreatorAssets.ButtonRedStyle;

            // Doing this here cause lazy lol.
            body.fontSize = 13;

            // Title.
            GUILayout.Label("Custom Models", header);

            GUILayout.Space(contentSpacing);

            // Description.
            GUILayout.Label("Use this toggle to enable your own custom models for pieces.", body);

            GUILayout.Space(textSpacing);

            GUILayout.Label("Insert prefabs with models for each desired piece below and then enable. Make sure you've assigned a model for every piece before enabling or it will not work.", body);

            GUILayout.Space(textSpacing);

            GUILayout.Label("Piece X and Y size scale must be 1 and 1 in Unity size units.", body);

            GUILayout.Space(contentSpacing * 2);

            // Button section.
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Get correct button style.
            GUIStyle enabledButton;

            if (selection.UserModelsEnabled) enabledButton = greenButton;
            else enabledButton = defaultButton;

            // Enabled button.
            if(GUILayout.Button("Enabled", enabledButton))
            {
                selection.EnableCustomModels();
            }

            GUILayout.Space(buttonSpacing);

            // Get correct button style.
            GUIStyle disabledButton;

            if (selection.UserModelsEnabled == false) disabledButton = redButton;
            else disabledButton = defaultButton;

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
