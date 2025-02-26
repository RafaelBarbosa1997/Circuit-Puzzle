using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    [ExecuteInEditMode]
    public class UserModelSelection : MonoBehaviour
    {
        #region FIELDS
        // Reference to assets for inspector.
        private SOAssetHolder assets;

        // Reference to user models.
        private UserModels userModels;

        // Reference to selection indicator.
        private GameObject selectionIndicator;

        // Bool that keeps track of whether user models are enabled or not.
        [SerializeField]
        private bool userModelsEnabled;
        #endregion

        #region PROPERTIES
        public bool UserModelsEnabled { get => userModelsEnabled; set => userModelsEnabled = value; }
        public SOAssetHolder Assets { get => assets; private set => assets = value; }
        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            // Get assets reference.
            assets = GetComponent<SOAssetHolder>();

            // Get user models reference.
            userModels = GetComponent<UserModels>();

            // Get selection indicator reference.
            selectionIndicator = transform.GetChild(3).gameObject;
        }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Enables user models for pieces and selection indicator.
        /// </summary>
        public void EnableCustomModels()
        {
            // Check if all custom models have been assigned.
            userModels.PopulateCheckList();
            foreach(GameObject piece in userModels.CheckList)
            {
                // If any models haven't been assigned, error message and return.
                if(piece == null)
                {
                    Debug.LogError("Please assign every piece model before enabling custom models");
                    return;
                }
            }

            // Default pieces.
            EnableDefaultPieces();

            // Selection Indicator.
            // Clear default model.
            GameObject defaultModel = selectionIndicator.transform.GetChild(0).gameObject;
            DestroyImmediate(defaultModel);

            // Set custom model.
            GameObject customModel = userModels.SelectionIndicator;
            Instantiate(customModel, selectionIndicator.transform);

            // Set custom models status to enabled.
            userModelsEnabled = true;
        }

        /// <summary>
        /// Disables all user models.
        /// </summary>
        public void DisableCustomModels()
        {
            // Default pieces.
            DisableDefaultPieces();

            // Selection indicator.
            // Clear custom model.
            GameObject customModel = selectionIndicator.transform.GetChild(0).gameObject;
            DestroyImmediate(customModel);

            // Set default model.
            GameObject defaultModel = assets.PieceAssets.SelectionIndicator;
            Instantiate(defaultModel, selectionIndicator.transform);

            // Set custom models status to disabled.
            userModelsEnabled = false;
        }
        #endregion

        #region PRIVATE METHODS
        /// <summary>
        /// Goes through each piece in the board and switches to custom models.
        /// </summary>
        private void EnableDefaultPieces()
        {
            // Get board transform.
            Transform boardTransform = transform.GetChild(0);

            // If there are no pieces, no need to do anything.
            if (boardTransform.childCount == 0) return;

            // Enable the custom user model for each piece in the puzzle.
            foreach (Transform child in boardTransform)
            {
                PieceSwitcher switcher = child.GetComponent<PieceSwitcher>();
                switcher.EnableUserModel();
            }
        }

        /// <summary>
        /// Goes through each piece in the board and switches to default models.
        /// </summary>
        private void DisableDefaultPieces()
        {
            // Get board transform.
            Transform boardTransform = transform.GetChild(0);

            // If there are no pieces, no need to do anything.
            if (boardTransform.childCount == 0) return;

            // Disable the custom user model for each piece in the puzzle.
            foreach (Transform child in boardTransform)
            {
                PieceSwitcher switcher = child.GetComponent<PieceSwitcher>();
                switcher.DisableUserModel();
            }
        }
        #endregion
    }
}
