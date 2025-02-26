using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CircuitPuzzle
{
    [CreateAssetMenu(fileName = "PuzzleCreatorAssets")]
    public class PuzzleCreatorAssetReferencesSO : ScriptableObject
    {
        // Textures.
        [SerializeField]
        private Texture2D leftArrow;
        [SerializeField] 
        private Texture2D rightArrow;

        // Styles.
        [SerializeField]
        private GUIStyle headerStyle;
        [SerializeField]
        private GUIStyle labelStyle;
        [SerializeField]
        private GUIStyle fieldStyle;

        [SerializeField]
        private GUIStyle buttonStyle;
        [SerializeField]
        private GUIStyle fieldChangesStyle;
        [SerializeField]
        private GUIStyle buttonGreenStyle;
        [SerializeField]
        private GUIStyle buttonRedStyle;
        [SerializeField]
        private GUIStyle checkBoxStyle;

        [SerializeField]
        private GUIStyle warningHeaderStyle;
        [SerializeField]
        private GUIStyle warningLabelStyle;
        [SerializeField]
        private GUIStyle warningFunctionStyle;

        // Prefabs.
        [SerializeField]
        private GameObject blankPiecePrefab;
        [SerializeField]
        private GameObject previewPiecePrefab;

        // Materials.
        [SerializeField]
        private Material greenBase;
        [SerializeField]
        private Material redBase;


        public GUIStyle LabelStyle { get => labelStyle; private set => labelStyle = value; }
        public GUIStyle FieldStyle { get => fieldStyle; private set => fieldStyle = value; }
        public GUIStyle ButtonStyle { get => buttonStyle; private set => buttonStyle = value; }
        public GUIStyle HeaderStyle { get => headerStyle; private set => headerStyle = value; }
        public Texture2D LeftArrow { get => leftArrow; private set => leftArrow = value; }
        public Texture2D RightArrow { get => rightArrow; private set => rightArrow = value; }
        public GameObject BlankPiecePrefab { get => blankPiecePrefab; private set => blankPiecePrefab = value; }
        public GUIStyle FieldChangesStyle { get => fieldChangesStyle; private set => fieldChangesStyle = value; }
        public GUIStyle ButtonGreenStyle { get => buttonGreenStyle; private set => buttonGreenStyle = value; }
        public GUIStyle ButtonRedStyle { get => buttonRedStyle; private set => buttonRedStyle = value; }
        public GUIStyle CheckBoxStyle { get => checkBoxStyle; private set => checkBoxStyle = value; }
        public GameObject PreviewPiecePrefab { get => previewPiecePrefab; private set => previewPiecePrefab = value; }
        public Material GreenBase { get => greenBase; private set => greenBase = value; }
        public Material RedBase { get => redBase; private set => redBase = value; }
        public GUIStyle WarningHeaderStyle { get => warningHeaderStyle; private set => warningHeaderStyle = value; }
        public GUIStyle WarningLabelStyle { get => warningLabelStyle; private set => warningLabelStyle = value; }
        public GUIStyle WarningFunctionStyle { get => warningFunctionStyle; private set => warningFunctionStyle = value; }
    }
}
