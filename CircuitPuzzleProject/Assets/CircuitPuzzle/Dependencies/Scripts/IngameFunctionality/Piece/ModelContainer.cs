using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    public class ModelContainer : MonoBehaviour
    {
        #region FIELDS
        // Default models.
        private List<GameObject> defaultUnpoweredModels;
        private List<GameObject> defaultPoweredModels;

        // Custom models.
        private List<GameObject> customUnpoweredModels;
        private List<GameObject> customPoweredModels;

        // Asset reference.
        private SOAssetHolder assetHolder;

        // Custom models reference.
        private UserModels userModels;
        #endregion

        #region PROPERTIES
        public List<GameObject> DefaultUnpoweredModels { get => defaultUnpoweredModels; private set => defaultUnpoweredModels = value; }
        public List<GameObject> DefaultPoweredModels { get => defaultPoweredModels; private set => defaultPoweredModels = value; }
        public List<GameObject> CustomUnpoweredModels { get => customUnpoweredModels; private set => customUnpoweredModels = value; }
        public List<GameObject> CustomPoweredModels { get => customPoweredModels; private set => customPoweredModels = value; }
        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            // Get assets.
            assetHolder = GetComponent<SOAssetHolder>();
            userModels = GetComponent<UserModels>();

            // Filler Gameobject.
            GameObject filler = new GameObject();

            // Populate lists.
            // Default Unpowered.
            defaultUnpoweredModels = new List<GameObject>
            {
                filler,
                assetHolder.PieceAssets.StraightPieceUnpowered,
                assetHolder.PieceAssets.TPieceUnpowered,
                assetHolder.PieceAssets.CornerPieceUnpowered,
                assetHolder.PieceAssets.StartPieceUnpowered,
                assetHolder.PieceAssets.EndPieceUnpowered
            };

            // Default Powered.
            defaultPoweredModels = new List<GameObject>
            {
                filler,
                assetHolder.PieceAssets.StraightPiecePowered,
                assetHolder.PieceAssets.TPiecePowered,
                assetHolder.PieceAssets.CornerPiecePowered,
                assetHolder.PieceAssets.StartPiecePowered,
                assetHolder.PieceAssets.EndPiecePowered
            };

            // Custom Unpowered.
            customUnpoweredModels = new List<GameObject>
            {
                userModels.BlankPiece,
                userModels.StraightPiece,
                userModels.TPiece,
                userModels.CornerPiece,
                userModels.StartPiece,
                userModels.EndPiece
            };

            // Custom powered.
            customPoweredModels = new List<GameObject>
            {
                filler,
                userModels.StraightPiecePowered,
                userModels.TPiecePowered,
                userModels.CornerPiecePowered,
                filler,
                userModels.EndPiecePowered
            };
        }
        #endregion
    }
}
