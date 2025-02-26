using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    [CreateAssetMenu(fileName = "PieceAssets")]
    public class PieceAssetReferencesSO : ScriptableObject
    {
        #region FIELDS
        // Styles.
        [Header("Styles")]
        [SerializeField]
        private GUIStyle inactiveButton;
        [SerializeField]
        private GUIStyle activeButton;

        // Prefabs.
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

        // Selection indicator.
        [Header("Selection Indicator")]
        [SerializeField]
        private GameObject selectionIndicator;

        // Unpowered models.
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
        #endregion

        #region PROPERTIES
        // Prefabs.
        public GUIStyle InactiveButton { get => inactiveButton; private set => inactiveButton = value; }
        public GUIStyle ActiveButton { get => activeButton; private set => activeButton = value; }
        public GameObject BlankPiece { get => blankPiece; private set => blankPiece = value; }
        public GameObject StraightPiece { get => straightPiece; private set => straightPiece = value; }
        public GameObject TPiece { get => tPiece; private set => tPiece = value; }
        public GameObject CornerPiece { get => cornerPiece; private set => cornerPiece = value; }
        public GameObject StartPiece { get => startPiece; private set => startPiece = value; }
        public GameObject EndPiece { get => endPiece; private set => endPiece = value; }
        public GameObject SelectionIndicator { get => selectionIndicator; private set => selectionIndicator = value; }

        // Unpowered.
        public GameObject StraightPieceUnpowered { get => straightPieceUnpowered; private set => straightPieceUnpowered = value; }
        public GameObject TPieceUnpowered { get => tPieceUnpowered; private set => tPieceUnpowered = value; }
        public GameObject CornerPieceUnpowered { get => cornerPieceUnpowered; private set => cornerPieceUnpowered = value; }
        public GameObject StartPieceUnpowered { get => startPieceUnpowered; private set => startPieceUnpowered = value; }
        public GameObject EndPieceUnpowered { get => endPieceUnpowered; private set => endPieceUnpowered = value; }

        // Powered.
        public GameObject StraightPiecePowered { get => straightPiecePowered; private set => straightPiecePowered = value; }
        public GameObject TPiecePowered { get => tPiecePowered; private set => tPiecePowered = value; }
        public GameObject CornerPiecePowered { get => cornerPiecePowered; private set => cornerPiecePowered = value; }
        public GameObject StartPiecePowered { get => startPiecePowered; private set => startPiecePowered = value; }
        public GameObject EndPiecePowered { get => endPiecePowered; private set => endPiecePowered = value; }
        #endregion
    }
}