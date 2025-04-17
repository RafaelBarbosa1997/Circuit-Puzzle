using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    public class SOAssetHolder : MonoBehaviour
    {
        [SerializeField]
        private InspectorAssetsSO inspectorAssets;
        [SerializeField]
        private PieceAssetsSO pieceAssets;

        public InspectorAssetsSO InspectorAssets { get => inspectorAssets; private set => inspectorAssets = value; }
        public PieceAssetsSO PieceAssets { get => pieceAssets; private set => pieceAssets = value; }
    }
}