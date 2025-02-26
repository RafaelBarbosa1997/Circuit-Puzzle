using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    [ExecuteInEditMode]
    public class GroupedEndingWarning : MonoBehaviour
    {
        #region FIELDS
        // Reference to assets used in inspector.
        [SerializeField]
        private SOAssetHolder assets;

        // Reference to puzzle settings to get group mode.
        [SerializeField]
        private PuzzleSettings puzzleSettings;
        #endregion

        #region PROPERTIES
        public PuzzleSettings PuzzleSettings { get => puzzleSettings; private set => puzzleSettings = value; }
        public SOAssetHolder Assets { get => assets; private set => assets = value; }

        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            // Get references.
            assets = GetComponent<SOAssetHolder>();
            puzzleSettings = GetComponent<PuzzleSettings>();
        }
        #endregion
    }
}
