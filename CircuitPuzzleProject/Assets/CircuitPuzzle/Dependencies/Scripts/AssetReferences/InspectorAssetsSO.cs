using UnityEngine;

namespace CircuitPuzzle
{
    [CreateAssetMenu(fileName = "CircuitPuzzleInspectorAssets", menuName = "Circuit Puzzle/Inspector Assets")]
    public class InspectorAssetsSO : ScriptableObject
    {
        #region FIELDS
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
        [SerializeField]
        private GUIStyle defaultButton;
        [SerializeField]
        private GUIStyle greenButton;
        [SerializeField]
        private GUIStyle redButton;
        [SerializeField]
        private GUIStyle inactiveButton;
        [SerializeField]
        private GUIStyle activeButton;

        // Checkbox style.
        [Header("Checkbox Style")]
        [SerializeField]
        private GUIStyle checkbox;
        #endregion

        #region PROPERTIES
        public Texture2D LeftArrow { get => leftArrow; private set => leftArrow = value; }
        public Texture2D RightArrow { get => rightArrow; private set => rightArrow = value; }
        public GUIStyle DefaultHeader { get => defaultHeader; private set => defaultHeader = value; }
        public GUIStyle GroupedWarningHeader { get => groupedWarningHeader; private set => groupedWarningHeader = value; }
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
        public GUIStyle Checkbox { get => checkbox; private set => checkbox = value; }
        #endregion
    }
}
