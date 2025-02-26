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
        // Styles.
        private GUIStyle headerStyle;
        private GUIStyle labelStyle;

        // Button styles.
        private GUIStyle activeButton;
        private GUIStyle inactiveButton;

        // Spacing
        private float spacing = 10;
        #endregion

        public override void OnInspectorGUI()
        {
            // Get script reference.
            PuzzleSettings settings = (PuzzleSettings)target;
            if(settings == null)
            {
                return;
            }

            // Get styles.
            headerStyle = settings.References.PuzzleCreatorAssets.HeaderStyle;
            labelStyle = settings.References.PuzzleCreatorAssets.LabelStyle;
            activeButton = settings.References.PieceAssets.ActiveButton;
            inactiveButton = settings.References.PieceAssets.InactiveButton;

            // TITLE.
            GUILayout.Label("Puzzle Settings", headerStyle);

            // Spacing //
            GUILayout.Space(spacing * 2);

            // SETTINGS.

            // Spacing // 
            GUILayout.Space(spacing);

            GUILayout.BeginVertical();

            // PUZZLE TYPE SECTION.
            // Title.
            GUILayout.Label("Puzzle Type", labelStyle);

            // Spacing //
            GUILayout.Space(spacing);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Set active button.
            GUIStyle continuous;
            GUIStyle oneTime;

            if (settings.OneTimeCompletion)
            {
                oneTime = activeButton;
                continuous = inactiveButton;
            }

            else
            {
                oneTime = inactiveButton;
                continuous = activeButton;
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
            GUILayout.Label("Group Mode", labelStyle);

            // Spacing //
            GUILayout.Space(spacing);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Set active button.
            GUIStyle endingSingle;
            GUIStyle endingGrouped;

            if (settings.IsGrouped)
            {
                endingSingle = inactiveButton;
                endingGrouped = activeButton;
            }

            else
            {
                endingSingle = activeButton;
                endingGrouped = inactiveButton;
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
            GUILayout.Label("Starter pieces", labelStyle);

            GUILayout.Space(spacing);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

                    // Set active button.
                    GUIStyle starterLocked;
                    GUIStyle starterUnlocked;

                    if (settings.LockStartingPieces)
                    {
                        starterLocked = activeButton;
                        starterUnlocked = inactiveButton;
                    }

                    else
                    {
                        starterLocked = inactiveButton;
                        starterUnlocked = activeButton;
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
            GUILayout.Label("Ending Pieces", labelStyle);

            // Spacing //
            GUILayout.Space(spacing);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Set active button.
            GUIStyle endingLocked;
            GUIStyle endingUnlocked;
            
            if(settings.LockEndingPieces)
            {
                endingLocked = activeButton;
                endingUnlocked = inactiveButton;
            }

            else
            {
                endingLocked = inactiveButton;
                endingUnlocked = activeButton;
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