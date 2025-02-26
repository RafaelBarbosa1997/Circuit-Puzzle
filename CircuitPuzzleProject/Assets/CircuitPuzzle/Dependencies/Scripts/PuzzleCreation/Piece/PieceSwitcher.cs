using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CircuitPuzzle
{
    [ExecuteInEditMode]
    public class PieceSwitcher : MonoBehaviour
    {
        #region FIELDS
        // Reference holder.
        private SOAssetHolder references;

        // Reference to user models.
        private UserModels userModels;

        // Index for this puzzle piece in the puzzle matrix.
        [SerializeField, HideInInspector]
        private int row;
        [SerializeField, HideInInspector]
        private int column;

        // Index to know which piece type this is, used for active button selection.
        [SerializeField, HideInInspector]
        private int typeIndex;
        #endregion

        #region PROPERTIES
        public SOAssetHolder References { get => references; private set => references = value; }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
        public int TypeIndex { get => typeIndex; set => typeIndex = value; }
        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            // Get asset references.
            references = GetComponent<SOAssetHolder>();

            // Get user model references.
            userModels = transform.parent.transform.parent.gameObject.GetComponent<UserModels>();

            // Set typeIndex.
            typeIndex = GetComponent<PieceBase>().TypeIndex;

            // If user models enabled, apply them.
            UserModelSelection selection = transform.parent.transform.parent.gameObject.GetComponent<UserModelSelection>();
            if (selection.UserModelsEnabled)
            {
                EnableUserModel();
            }
        }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        ///  Switches this pieces type for another while keeping all its settings intact.
        /// </summary>
        /// <param name="piece"></param>
        public void SwitchPiece(GameObject piece, int typeIndex)
        {
            // Get reference to the puzzle matrix this piece is contained in.
            GameObject[,] matrix = transform.parent.parent.GetComponent<PuzzleCreator>().PuzzlePieces;

            // Instantiate the new piece.
            GameObject newPiece = Instantiate(piece, transform.parent.transform);

            // Replace this piece from the puzzle matrix with the new piece.
            matrix[row, column] = newPiece;

            // Transfer this piece's information and components to the new piece.
            // Basic info.
            newPiece.name = name;
            newPiece.transform.localPosition = transform.localPosition;
            newPiece.transform.SetSiblingIndex(transform.GetSiblingIndex());

            // Switcher
            PieceSwitcher newSwitcher = newPiece.GetComponent<PieceSwitcher>();
            newSwitcher.Row = row;
            newSwitcher.Column = column;

            // Set scene as dirty so changes can be saved.
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            // Destroy this piece.
            DestroyImmediate(gameObject);
        }

        /// <summary>
        /// Enables the user model for this piece, while disabling the default model.
        /// </summary>
        public void EnableUserModel()
        {
            // Get correct transform to parent user model to.
            Transform transformParent;
            if (typeIndex > 0) transformParent = transform.GetChild(2);
            else transformParent = transform.GetChild(1);

            // If the transform already has a child means we are reloading scene and no need to do anything.
            if (transformParent.childCount > 0) return;

            // Populate the custom model list before retrieving model.
            userModels.PopulatePieceList();

            // Get correct model from type index.
            GameObject model = userModels.Pieces[typeIndex];

            // If the user hasn't set their custom models.
            if (model == null)
            {
                Debug.LogError("User models are enabled but not all user models have been assigned in inspector.");
                Debug.LogError("Please disable custom models and reenable them once all models have been assigned.");

                return;
            }

            // Get default model and disable it.
            GameObject original = transform.GetChild(0).gameObject;
            original.SetActive(false);

            // Instantiate user model.
            Instantiate(model, transformParent);
        }

        /// <summary>
        /// Disables the user model on this piece, while enabling the default model
        /// </summary>
        public void DisableUserModel()
        {
            // Get correct child index for user model.
            int childIndex;
            if (typeIndex > 0) childIndex = 2;
            else childIndex = 1;

            // Get user model parent.
            GameObject userModelContainer = transform.GetChild(childIndex).gameObject;

            // If parent is empty nothing needs to be done, return.
            if (userModelContainer.transform.childCount == 0) return;

            // Otherwise destroy user model.
            GameObject userModel = userModelContainer.transform.GetChild(0).gameObject;

            DestroyImmediate(userModel);

            // Reenable default model.
            GameObject defaultModel = transform.GetChild(0).gameObject;
            defaultModel.SetActive(true);
        }
        #endregion
    }
}
