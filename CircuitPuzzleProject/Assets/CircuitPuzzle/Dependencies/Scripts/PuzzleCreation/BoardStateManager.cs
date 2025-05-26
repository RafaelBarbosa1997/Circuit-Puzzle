using UnityEngine;

namespace CircuitPuzzle
{
    /// <summary>
    /// Holds the matrix with references to the currently saved puzzle instance's pieces.
    /// Acts as a centralized access point for other classes to access and modify the board state.
    /// </summary>
    [ExecuteInEditMode]
    public class BoardStateManager : MonoBehaviour
    {
        #region FIELDS
        [SerializeField]
        private Transform boardTransform;

        private GameObject[,] puzzlePieces;
        #endregion

        #region PROPERTIES
        public GameObject[,] PuzzlePieces { get => puzzlePieces; set => puzzlePieces = value; }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Rebuils the puzzle piece matrix by accessing the puzzle piece GameObject children in boardTransform.
        /// This is called from <see cref="PuzzleCreator"> during initialization, only when a saved puzzle instance exists.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        public void RebuildMatrixFromBoard(int rows, int columns)
        {
            puzzlePieces = new GameObject[rows, columns];

            // Each individual piece contains its own position in the matrix.
            // Therefore, we can access that info to assign it the correct position in the matrix.
            for (int i = 0; i < boardTransform.childCount; i++)
            {
                PieceSwitcher switcher = boardTransform.GetChild(i).GetComponent<PieceSwitcher>();

                puzzlePieces[switcher.Row, switcher.Column] = switcher.gameObject;
            }
        }
        #endregion
    }
}
