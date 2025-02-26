using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    public class SOAssetHolder : MonoBehaviour
    {
        [SerializeField]
        private PuzzleCreatorAssetReferencesSO puzzleCreatorAssets;
        [SerializeField]
        private PieceAssetReferencesSO pieceAssets;

        public PuzzleCreatorAssetReferencesSO PuzzleCreatorAssets { get => puzzleCreatorAssets; private set => puzzleCreatorAssets = value; }
        public PieceAssetReferencesSO PieceAssets { get => pieceAssets; private set => pieceAssets = value; }
    }
}