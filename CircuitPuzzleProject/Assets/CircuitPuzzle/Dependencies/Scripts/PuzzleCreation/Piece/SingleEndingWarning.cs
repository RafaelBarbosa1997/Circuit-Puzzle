using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    [ExecuteInEditMode]
    public class SingleEndingWarning : MonoBehaviour
    {
        #region FIELDS
        // Reference to assets used in inspector.
        [SerializeField]
        private PuzzleAssetsHolder assets;

        // Reference to puzzle settings to get group mode.
        [SerializeField]
        private PuzzleSettings puzzleSettings;
        #endregion

        #region PROPERTIES
        public PuzzleSettings PuzzleSettings { get => puzzleSettings; private set => puzzleSettings = value; }
        public PuzzleAssetsHolder Assets { get => assets; private set => assets = value; }

        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            // Get references.
            assets = GetComponent<PuzzleAssetsHolder>();

            // Get PuzzleSettings reference.
            Transform pieceParent = transform.parent;
            Transform boardParent = pieceParent.parent;
            puzzleSettings = boardParent.gameObject.GetComponent<PuzzleSettings>();
        }
        #endregion
    }
}
