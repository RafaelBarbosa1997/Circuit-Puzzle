using UnityEngine;

namespace CircuitPuzzle
{
    /// <summary>
    /// Holds references to the assets and values used in the custom editors.
    /// This includes GUIStyles and textures used in the editors' GUI layout.
    /// </summary>
    [CreateAssetMenu(fileName = "CircuitPuzzleInspectorAssets", menuName = "Circuit Puzzle/Inspector Assets")]
    public class InspectorAssetsSO : ScriptableObject
    {
        #region FIELDS
        // Spacing values.
        [Header("Spacing Values")]
        [SerializeField]
        private float contentSpacing;
        [SerializeField]
        private float groupSpacing;

        // Arrow textures.
        [Header("Arrow Textures")]
        [SerializeField]
        private Texture2D leftArrow;
        [SerializeField]
        private Texture2D rightArrow;

        // Header styles.
        [Header("Header Styles")]
        [SerializeField]
        private GUIStyle defaultHeader;
        [SerializeField]
        private GUIStyle groupedWarningHeader;
        [SerializeField]
        private GUIStyle invalidStateHeader;

        // Label styles.
        [Header("Label Styles")]
        [SerializeField]
        private GUIStyle defaultLabel;
        [SerializeField]
        private GUIStyle groupedWarningLabel;
        [SerializeField]
        private GUIStyle groupedWarningSubLabel;
        [SerializeField]
        private GUIStyle userModelLabel;

        // Field styles.
        [Header("Field Styles")]
        [SerializeField]
        private GUIStyle defaultField;
        [SerializeField]
        private GUIStyle changesPendingField;

        // Button styles.
        [Header("Button Styles")]
        [Header("General Buttons")]
        [SerializeField]
        private GUIStyle inactiveButton;
        [SerializeField]
        private GUIStyle activeButton;
        [SerializeField]
        private GUIStyle activeButtonRed;
        [Header("Custom Models Buttons")]
        [SerializeField]
        private GUIStyle defaultButton;
        [SerializeField]
        private GUIStyle greenButton;
        [SerializeField]
        private GUIStyle redButton;

        // Checkbox style.
        [Header("Checkbox Style")]
        [SerializeField]
        private GUIStyle checkbox;
        #endregion

        #region PROPERTIES
        public float ContentSpacing { get => contentSpacing; private set => contentSpacing = value; }
        public float GroupSpacing { get => groupSpacing; private set => groupSpacing = value; }
        public Texture2D LeftArrow { get => leftArrow; private set => leftArrow = value; }
        public Texture2D RightArrow { get => rightArrow; private set => rightArrow = value; }
        public GUIStyle DefaultHeader { get => defaultHeader; private set => defaultHeader = value; }
        public GUIStyle GroupedWarningHeader { get => groupedWarningHeader; private set => groupedWarningHeader = value; }
        public GUIStyle InvalidStateHeader { get => invalidStateHeader; private set => invalidStateHeader = value; }
        public GUIStyle DefaultLabel { get => defaultLabel; private set => defaultLabel = value; }
        public GUIStyle GroupedWarningLabel { get => groupedWarningLabel; private set => groupedWarningLabel = value; }
        public GUIStyle GroupedWarningSubLabel { get => groupedWarningSubLabel; private set => groupedWarningSubLabel = value; }
        public GUIStyle UserModelLabel { get => userModelLabel; private set => userModelLabel = value; }
        public GUIStyle DefaultField { get => defaultField; private set => defaultField = value; }
        public GUIStyle ChangesPendingField { get => changesPendingField; private set => changesPendingField = value; }
        public GUIStyle DefaultButton { get => defaultButton; private set => defaultButton = value; }
        public GUIStyle GreenButton { get => greenButton; private set => greenButton = value; }
        public GUIStyle RedButton { get => redButton; private set => redButton = value; }
        public GUIStyle InactiveButton { get => inactiveButton; private set => inactiveButton = value; }
        public GUIStyle ActiveButton { get => activeButton; private set => activeButton = value; }
        public GUIStyle ActiveButtonRed { get => activeButtonRed; private set => activeButtonRed = value; }
        public GUIStyle Checkbox { get => checkbox; private set => checkbox = value; }
        #endregion
    }
}
