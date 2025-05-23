using UnityEngine;

namespace CircuitPuzzle
{
    /// <summary>
    /// This class serves to hold references to the assets and values used in the puzzle.
    /// This includes assets and values to be used in the custom editor, as well as for the actual puzzle's functionality.
    /// Other scripts access this class through GetComponent().
    /// </summary>
    public class PuzzleAssetsHolder : MonoBehaviour
    {
        [SerializeField]
        private Transform boardTransform;
        [SerializeField]
        private Transform previewTransform;

        [SerializeField]
        private InspectorAssetsSO inspectorAssets;
        [SerializeField]
        private PieceAssetsSO pieceAssets;

        public Transform BoardTransform { get => boardTransform; private set => boardTransform = value; }
        public Transform PreviewTransform { get => previewTransform; private set => previewTransform = value; }
        public InspectorAssetsSO InspectorAssets { get => inspectorAssets; private set => inspectorAssets = value; }
        public PieceAssetsSO PieceAssets { get => pieceAssets; private set => pieceAssets = value; }
    }
}