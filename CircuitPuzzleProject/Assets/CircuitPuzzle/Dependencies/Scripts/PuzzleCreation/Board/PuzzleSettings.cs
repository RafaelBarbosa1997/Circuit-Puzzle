using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    [ExecuteInEditMode]
    public class PuzzleSettings : MonoBehaviour
    {
        #region FIELDS
        // Reference to assets used in inspector.
        [SerializeField]
        private SOAssetHolder references;

        // Boolean that determines whether the rotation of starting pieces is blocked.
        [SerializeField]
        private bool lockStartingPieces;

        // Boolean that determines whether the rotation of ending pieces is blocked.
        [SerializeField]
        private bool lockEndingPieces;

        // Boolean that determines whether this puzzle is continuous or one time completion.
        [SerializeField]
        private bool oneTimeCompletion;

        // Boolean that determines whether this puzzle's ending pieces are grouped or single instances.
        [SerializeField]
        private bool isGrouped;
        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            // Get references.
            references = GetComponent<SOAssetHolder>();
        }
        #endregion

        #region PROPERTIES
        public bool LockStartingPieces { get => lockStartingPieces;  set => lockStartingPieces = value; }
        public bool LockEndingPieces { get => lockEndingPieces;  set => lockEndingPieces = value; }
        public bool OneTimeCompletion { get => oneTimeCompletion;  set => oneTimeCompletion = value; }
        public SOAssetHolder References { get => references; private set => references = value; }
        public bool IsGrouped { get => isGrouped; set => isGrouped = value; }
        #endregion
    }
}