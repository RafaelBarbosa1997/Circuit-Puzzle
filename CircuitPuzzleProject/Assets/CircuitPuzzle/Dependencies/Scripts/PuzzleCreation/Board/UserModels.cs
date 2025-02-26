using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    [ExecuteInEditMode]
    public class UserModels : MonoBehaviour
    {
        #region FIELDS
        // Model references.
        [Header("Selection Indicator")]
        [SerializeField]
        private GameObject selectionIndicator;

        [Header("Pieces default state")]
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

        [Header("Pieces powered state")]
        [SerializeField]
        private GameObject straightPiecePowered;
        [SerializeField]
        private GameObject tPiecePowered;
        [SerializeField]
        private GameObject cornerPiecePowered;
        [SerializeField]
        private GameObject endPiecePowered;

        // List so piece switcher can get references by type.
        private List<GameObject> pieces;

        // List for correct assignment check.
        private List<GameObject> checkList;
        #endregion

        #region PROPERTIES
        // Misc.
        public List<GameObject> Pieces { get => pieces; private set => pieces = value; }
        public GameObject SelectionIndicator { get => selectionIndicator; private set => selectionIndicator = value; }
        public List<GameObject> CheckList { get => checkList; private set => checkList = value; }

        // Unpowered models.
        public GameObject BlankPiece { get => blankPiece; private set => blankPiece = value; }
        public GameObject StraightPiece { get => straightPiece; private set => straightPiece = value; }
        public GameObject TPiece { get => tPiece; private set => tPiece = value; }
        public GameObject CornerPiece { get => cornerPiece; private set => cornerPiece = value; }
        public GameObject StartPiece { get => startPiece; private set => startPiece = value; }
        public GameObject EndPiece { get => endPiece; private set => endPiece = value; }

        // Powered models.
        public GameObject StraightPiecePowered { get => straightPiecePowered; private set => straightPiecePowered = value; }
        public GameObject TPiecePowered { get => tPiecePowered; private set => tPiecePowered = value; }
        public GameObject CornerPiecePowered { get => cornerPiecePowered; private set => cornerPiecePowered = value; }
        public GameObject EndPiecePowered { get => endPiecePowered; private set => endPiecePowered = value; }
        #endregion

        #region PUBLIC METHODS
        public void PopulatePieceList()
        {
            pieces = new List<GameObject>()
            {
                blankPiece,
                straightPiece,
                tPiece,
                cornerPiece,
                startPiece,
                endPiece
            };
        }

        public void PopulateCheckList()
        {
            checkList = new List<GameObject>()
            {
                selectionIndicator,
                blankPiece,
                straightPiece,
                tPiece,
                cornerPiece,
                startPiece,
                endPiece,
                straightPiecePowered,
                tPiecePowered,
                cornerPiecePowered,
                endPiecePowered,
            };
        }
        #endregion
    }
}
