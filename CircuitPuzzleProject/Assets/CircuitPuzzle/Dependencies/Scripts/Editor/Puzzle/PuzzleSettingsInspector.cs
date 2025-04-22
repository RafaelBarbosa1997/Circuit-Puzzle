using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Codice.Client.Common.GameUI;
using UnityEditor.SceneManagement;

namespace CircuitPuzzle
{
    [CustomEditor(typeof(PuzzleSettings))]
    public class PuzzleSettingsInspector : Editor
    {
        #region FIELDS
        // Spacing
        private const float spacing = 10;
        #endregion

        public override void OnInspectorGUI()
        {
            // Get script reference.
            PuzzleSettings settings = (PuzzleSettings)target;
            if(settings == null)
            {
                return;
            }

            // Get reference to the inspector assets for custom editor.
            InspectorAssetsSO inspectorAssets = settings.GetComponent<PuzzleAssetsHolder>().InspectorAssets;

            // TITLE.
            GUILayout.Label("Puzzle Settings", inspectorAssets.DefaultHeader);

            // Spacing //
            GUILayout.Space(spacing * 2);

            // SETTINGS.

            // Spacing // 
            GUILayout.Space(spacing);

            GUILayout.BeginVertical();

            // PUZZLE TYPE SECTION.
            // Title.
            GUILayout.Label("Puzzle Type", inspectorAssets.DefaultLabel);

            // Spacing //
            GUILayout.Space(spacing);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Set active button.
            GUIStyle continuous;
            GUIStyle oneTime;

            if (settings.OneTimeCompletion)
            {
                oneTime = inspectorAssets.ActiveButton;
                continuous = inspectorAssets.InactiveButton;
            }

            else
            {
                oneTime = inspectorAssets.InactiveButton;
                continuous = inspectorAssets.ActiveButton;
            }

            // Continuous button.
            if (GUILayout.Button("Continuous", continuous))
            {
                settings.OneTimeCompletion = false;

                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            // Spacing //
            GUILayout.Space(spacing);


            // One time button.
            if (GUILayout.Button("One Time", oneTime))
            {
                settings.OneTimeCompletion = true;

                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            // Spacing //
            GUILayout.Space(spacing * 2);

            //ENDING GROUP SECTION.
            // Title.
            GUILayout.Label("Group Mode", inspectorAssets.DefaultLabel);

            // Spacing //
            GUILayout.Space(spacing);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Set active button.
            GUIStyle endingSingle;
            GUIStyle endingGrouped;

            if (settings.IsGrouped)
            {
                endingSingle = inspectorAssets.InactiveButton;
                endingGrouped = inspectorAssets.ActiveButton;
            }

            else
            {
                endingSingle = inspectorAssets.ActiveButton;
                endingGrouped = inspectorAssets.InactiveButton;
            }

            // Single button.
            if (GUILayout.Button("Grouped", endingGrouped))
            {
                settings.IsGrouped = true;

                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            // Spacing //
            GUILayout.Space(spacing);

            // Grouped button.
            if (GUILayout.Button("Single", endingSingle))
            {
                settings.IsGrouped = false;

                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            // Spacing //
            GUILayout.Space(spacing * 2);

            // LOCK STARTER SECTION.
            // Title.
            GUILayout.Label("Starter pieces", inspectorAssets.DefaultLabel);

            GUILayout.Space(spacing);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

                    // Set active button.
                    GUIStyle starterLocked;
                    GUIStyle starterUnlocked;

                    if (settings.LockStartingPieces)
                    {
                        starterLocked = inspectorAssets.ActiveButton;
                        starterUnlocked = inspectorAssets.InactiveButton;
                    }

                    else
                    {
                        starterLocked = inspectorAssets.InactiveButton;
                        starterUnlocked = inspectorAssets.ActiveButton;
                    }
               
                    // Locked button.
                    if(GUILayout.Button("Locked", starterLocked))
                    {
                        settings.LockStartingPieces = true;

                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    }

                    GUILayout.Space(spacing);
            
                    // Unlocked button.
                    if(GUILayout.Button("Unlocked", starterUnlocked))
                    {
                        settings.LockStartingPieces = false;

                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // Spacing //
            GUILayout.Space(spacing * 2);

            // LOCK ENDING SECTION.
            // Title.
            GUILayout.Label("Ending Pieces", inspectorAssets.DefaultLabel);

            // Spacing //
            GUILayout.Space(spacing);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Set active button.
            GUIStyle endingLocked;
            GUIStyle endingUnlocked;
            
            if(settings.LockEndingPieces)
            {
                endingLocked = inspectorAssets.ActiveButton;
                endingUnlocked = inspectorAssets.InactiveButton;
            }

            else
            {
                endingLocked = inspectorAssets.InactiveButton;
                endingUnlocked = inspectorAssets.ActiveButton;
            }
            
            // Locked button.
            if(GUILayout.Button("Locked", endingLocked))
            {
                settings.LockEndingPieces = true;

                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            // Spacing //
            GUILayout.Space(spacing);

            // Unlocked button.
            if(GUILayout.Button("Unlocked", endingUnlocked))
            {
                settings.LockEndingPieces = false;

                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}