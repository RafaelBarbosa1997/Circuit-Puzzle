using UnityEngine;

namespace CircuitPuzzle
{
    /// <summary>
    /// This class serves to hold references to the assets and values used in the puzzle's creation and in-game functionality.
    /// </summary>
    [CreateAssetMenu(fileName = "CircuitPuzzlePieceAssets", menuName = "Circuit Puzzle/Piece Assets")]
    public class PieceAssetsSO : ScriptableObject
    {
        #region FIELDS
        // Piece prefabs.
        [Header("Piece Prefabs")]
        [SerializeField]
        private GameObject blankPiece;
        [SerializeField]
        private GameObject straightPiece;
        [SerializeField]
        private GameObject tPiece;
        [SerializeField]
        private GameObject cornerPiece;
        [SerializeField]
        private GameObject startPiece;
        [SerializeField]
        private GameObject endPiece;
        [SerializeField]
        private GameObject previewPiece;

        // Selection indicator.
        [Header("Selection Indicator Prefab")]
        [SerializeField]
        private GameObject selectionIndicator;

        // Default piece models.
        // Unpowered models.
        [Header("Default Piece Models")]
        [Header("Unpowered Models")]
        [SerializeField]
        private GameObject straightPieceUnpowered;
        [SerializeField]
        private GameObject tPieceUnpowered;
        [SerializeField]
        private GameObject cornerPieceUnpowered;
        [SerializeField]
        private GameObject startPieceUnpowered;
        [SerializeField]
        private GameObject endPieceUnpowered;

        // Powered models.
        [Header("Powered Models")]
        [SerializeField]
        private GameObject straightPiecePowered;
        [SerializeField]
        private GameObject tPiecePowered;
        [SerializeField]
        private GameObject cornerPiecePowered;
        [SerializeField]
        private GameObject startPiecePowered;
        [SerializeField]
        private GameObject endPiecePowered;

        // Materials.
        [Header("Materials")]
        [SerializeField]
        private Material greenPreviewMat;
        [SerializeField]
        private Material redPreviewMat;
        #endregion

        #region PROPERTIES
        public GameObject BlankPiece { get => blankPiece; private set => blankPiece = value; }
        public GameObject StraightPiece { get => straightPiece; private set => straightPiece = value; }
        public GameObject TPiece { get => tPiece; private set => tPiece = value; }
        public GameObject CornerPiece { get => cornerPiece; private set => cornerPiece = value; }
        public GameObject StartPiece { get => startPiece; private set => startPiece = value; }
        public GameObject EndPiece { get => endPiece; private set => endPiece = value; }
        public GameObject PreviewPiece { get => previewPiece; private set => previewPiece = value; }
        public GameObject SelectionIndicator { get => selectionIndicator; private set => selectionIndicator = value; }
        public GameObject StraightPieceUnpowered { get => straightPieceUnpowered; private set => straightPieceUnpowered = value; }
        public GameObject TPieceUnpowered { get => tPieceUnpowered; private set => tPieceUnpowered = value; }
        public GameObject CornerPieceUnpowered { get => cornerPieceUnpowered; private set => cornerPieceUnpowered = value; }
        public GameObject StartPieceUnpowered { get => startPieceUnpowered; private set => startPieceUnpowered = value; }
        public GameObject EndPieceUnpowered { get => endPieceUnpowered; private set => endPieceUnpowered = value; }
        public GameObject StraightPiecePowered { get => straightPiecePowered; private set => straightPiecePowered = value; }
        public GameObject TPiecePowered { get => tPiecePowered; private set => tPiecePowered = value; }
        public GameObject CornerPiecePowered { get => cornerPiecePowered; private set => cornerPiecePowered = value; }
        public GameObject StartPiecePowered { get => startPiecePowered; private set => startPiecePowered = value; }
        public GameObject EndPiecePowered { get => endPiecePowered; private set => endPiecePowered = value; }
        public Material GreenPreviewMat { get => greenPreviewMat; private set => greenPreviewMat = value; }
        public Material RedPreviewMat { get => redPreviewMat; private set => redPreviewMat = value; }
        #endregion
    }
}
