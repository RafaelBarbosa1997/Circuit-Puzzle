using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CircuitPuzzle
{
    [ExecuteInEditMode]
    public class PieceRotator : MonoBehaviour
    {
        #region FIELDS
        // Reference to inspector assets.
        private SOAssetHolder references;

        // Value that stores the current orientation.
        private float pieceOrientation;
        #endregion

        #region PROPERTIES
        public SOAssetHolder References { get => references; private set => references = value; }
        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            // Get reference to inspector assets.
            references = GetComponent<SOAssetHolder>();
        }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Rotates the puzzle piece counterclockwise.
        /// </summary>
        public void RotateLeft()
        {
            // Rotate on Z axis.
            pieceOrientation += 90;

            // Apply rotation.
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, pieceOrientation);

            // Mark scene as dirty so changes can be saved.
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        /// <summary>
        /// Rotates the puzzle pice clockwise.
        /// </summary>
        public void RotateRight()
        {
            // Rotate on Z axis.
            pieceOrientation -= 90;

            // Apply rotation.
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, pieceOrientation);

            // Mark scene as dirty so changes can be saved.
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
        #endregion
    }
}