using UnityEditor;

namespace CircuitPuzzle
{
    /// <summary>
    /// Utility class to disable interaction with custom editors during specific states.
    /// </summary>
    public static class InspectorInvalidStateDisplayer
    {
        /// <summary>
        /// Is called by custom editors during play mode.
        /// Replaces custom editor with warning, indicating its incompatible with play mode.
        /// </summary>
        /// <param name="inspectorAssets"></param>
        public static void DisplayPlayModeState(InspectorAssetsSO inspectorAssets)
        {
            EditorGUILayout.LabelField("Puzzle cannot be edited during play mode!", inspectorAssets.InvalidStateHeader);
        }

        /// <summary>
        /// Is called by custom editors when viewing in prefab preview mode.
        /// Replaces custom editor with warning, indicating its incompatible with prefab preview mode.
        /// </summary>
        /// <param name="inspectorAssets"></param>
        public static void DisplayPrefabPreviewState(InspectorAssetsSO inspectorAssets)
        {
            EditorGUILayout.LabelField("Puzzle cannot be edited in prefab preview mode!", inspectorAssets.InvalidStateHeader);

            EditorGUILayout.Space(inspectorAssets.GroupSpacing);

            EditorGUILayout.LabelField("Please open prefab for full editing capabilities", inspectorAssets.DefaultLabel);
        }
    }
}
