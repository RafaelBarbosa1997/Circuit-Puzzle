using UnityEngine;
using UnityEditor;

namespace CircuitPuzzle
{
    /// <summary>
    /// Responsible for creating and modifying puzzle instances in the editor.
    /// Also handles the generation of preview pieces, allowing users to visualize changes before applying them.
    /// While this class manages all creation and modification logic, the resulting puzzle matrix is stored in <see cref="BoardStateManager">,
    /// which serves as the centralized access point for puzzle state.
    /// </summary>
    [ExecuteInEditMode]
    public class PuzzleCreator : MonoBehaviour
    {
        #region FIELDS
        // Number of rows and columns the currently saved puzzle instance contains.
        [SerializeField]
        private int setRows;
        [SerializeField]
        private int setColumns;
        // Defines the maximum number of rows and columns allowed when limiter is enabled.
        [SerializeField]
        private int limiterValue;
        // Defines whether the number of allowed rows and columns are limited.
        [SerializeField]
        private bool isLimited;

        // Number of rows and columns currently inputted in the inspector.
        private int selectedRows;
        private int selectedColumns;

        // Serves as parent to puzzle pieces.
        private Transform boardTransform;
        // Serves as parent to preview pieces.
        private Transform previewTransform;
        // Stores preview pieces' references.
        private GameObject[,] previewPieces;

        // Holds references to the prefabs used to instantiate puzzle pieces.
        private PieceAssetsSO pieceAssets;
        // Contains matrix where references to instantiated puzzle pieces are stored.
        // Setup this way so board state is centralized, allowing classes to access piece references from a single access point.
        private BoardStateManager boardStateManager;
        #endregion

        #region PROPERTIES
        /// <summary>
        /// Clamps value for selectedRows and selectedColumns.
        /// Ensures minimum value is respected, as well as maximum value when limiter is enabled.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int ClampSelectedRowColValues(int value)
        {
            // Puzzles can't be created with a value smaller than 1.
            if (value < 1)
            {
                return 1;
            }

            // When limiter is enabled, value can't exceed its imposed limit.
            else if (value > limiterValue && isLimited)
            {
                return limiterValue;
            }

            else
            {
                return value;
            }
        }

        public int SelectedRows
        {
            get { return selectedRows; }

            set
            {
                selectedRows = ClampSelectedRowColValues(value);
            }
        }

        public int SelectedColumns
        {
            get { return selectedColumns; }

            set
            {
                selectedColumns = ClampSelectedRowColValues(value);
            }
        }

        public int SetRows { get => setRows; private set => setRows = value; }
        public int SetColumns { get => setColumns; private set => setColumns = value; }
        public int LimiterValue { get => limiterValue; private set => limiterValue = value; }
        public bool IsLimited { get => isLimited; private set => isLimited = value; }
        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            // Get necessary references from PuzzleAssetHolder.
            // This implementation is due to the use of a custom editor for this class, which doesn't allow for direct manual assignment.
            PuzzleAssetsHolder assetHolder = GetComponent<PuzzleAssetsHolder>();
            pieceAssets = assetHolder.PieceAssets;
            boardTransform = assetHolder.BoardTransform;
            previewTransform = assetHolder.PreviewTransform;

            boardStateManager = GetComponent<BoardStateManager>();

            if (Application.isPlaying)
            {
                // Previews shouldn't persist to play mode.
                // Bool will tell us if a preview did exist and was deleted.
                bool previewDeleted = DeletePreviewOnInitialization();

                // If a saved puzzle instance exists.
                if (setRows > 0 || setColumns > 0)
                {
                    // PuzzleCreator calls BoardStateManager's initialization, since its dependant on it.
                    boardStateManager.RebuildMatrixFromBoard(setRows, setColumns);

                    // Puzzle piece positions are changed to match preview when one is generated.
                    // So if preview was deleted, we need to reset puzzle piece's to their correct position.
                    if (previewDeleted)
                    {
                        SetPiecePositions(boardStateManager.PuzzlePieces, setRows, setColumns);
                    }
                }

                // Puzzle is not meant to be edited during play mode.
                // Nothing besides the previous setup should run.
                return;
            }

            // If a saved puzzle instance doesn't exist,
            // values for selected rows and columns need to be reset to default value, and an initial preview needs to be generated.
            if (setRows == 0 || setColumns == 0)
            {
                selectedRows = 1;
                selectedColumns = 1;

                // If user saved scene containing a PuzzleCreator with no instantiated puzzle instance, preview pieces will be saved in the scene.
                // Preview pieces are not meant to be persistent (and previewPieces matrix is not serialized).
                // So, existing preview pieces on scene load need to be deleted, before generating new preview for the default values.
                DeletePreviewOnInitialization();

                CreatePreview();
            }

            // If a saved puzzle instance exists,
            // values for selected rows and columns needs to be equal to instance's set rows and columns.
            // Additionally, puzzlePieces matrix in BoardStateManager needs to be populated with existing puzzle pieces.
            else
            {
                selectedRows = setRows;
                selectedColumns = setColumns;

                // BoardStateManager's initialization is called from here to simplify initialization order and create looser coupling.
                // We send in setRows and setColumns so its puzzle piece matrix is initialized with the correct size for the puzzle instance.
                boardStateManager.RebuildMatrixFromBoard(setRows, setColumns);

                // When a scene with a saved puzzle instance is loaded, the puzzle is in a state where a preview should not exist.
                // Previews are not meant to be persistent, but an unrelated action can dirty the scene while a preview is active, causing it to be saved to the scene.
                // If this happened, the existing preview needs to be deleted on scene load.
                if (DeletePreviewOnInitialization())
                {
                    // When a preview is generated, puzzle piece positions are altered to match the preview.
                    // So, if a preview existed and was deleted, we need to reset the puzzle pieces to their correct position for current puzzle instance.
                    SetPiecePositions(boardStateManager.PuzzlePieces, boardStateManager.PuzzlePieces.GetLength(0), boardStateManager.PuzzlePieces.GetLength(1));
                }
            }
        }
        #endregion

        #region PRIVATE METHODS

        #region PUZZLE CREATION
        /// <summary>
        /// Generates a new puzzle instance by instantiating individual piece prefabs.
        /// Creates the first puzzle iteration.
        /// Puzzle size is defined by user in inspector, by choosing selectedRows and selectedColumns values.
        /// </summary>
        private void CreatePuzzle()
        {
            // This matrix will store puzzle piece GameObject references, and can be accessed later to modify the puzzle.
            // Use of a matrix simplifies puzzle piece access, as it matches the actual puzzle layout.
            boardStateManager.PuzzlePieces = new GameObject[selectedRows, selectedColumns];

            for (int i = 0; i < boardStateManager.PuzzlePieces.GetLength(0); i++)
            {
                for (int j = 0; j < boardStateManager.PuzzlePieces.GetLength(1); j++)
                {
                    boardStateManager.PuzzlePieces[i, j] = Instantiate(pieceAssets.BlankPiece, boardTransform);

                    // Feed the piece its own position in the matrix so it can be switched to different piece types later.
                    SetPieceIndex(boardStateManager.PuzzlePieces[i, j], i, j);
                }
            }
        }

        /// <summary>
        /// Is called instead of CreatePuzzle, when a puzzle instance already exists.
        /// Modifies the current puzzle instance, adding or deleting rows and columns, based on user input in the inspector.
        /// Puzzle pieces that remain from previous instance are unnafected.
        /// </summary>
        private void ModifyPuzzle()
        {
            // Old instance needs to be stored in a temporary variable, so we can use it to delete pieces that aren't part of the new instance.
            GameObject[,] oldPuzzlePieces = boardStateManager.PuzzlePieces;

            boardStateManager.PuzzlePieces = new GameObject[selectedRows, selectedColumns];

            // First loop is through current matrix, to add pieces or reassign references for pieces retained from previous instance.
            for (int i = 0; i < boardStateManager.PuzzlePieces.GetLength(0); i++)
            {
                for (int j = 0; j < boardStateManager.PuzzlePieces.GetLength(1); j++)
                {
                    // If the previous instance reached this index, these pieces remain in the new instance.
                    if (i < oldPuzzlePieces.GetLength(0) && j < oldPuzzlePieces.GetLength(1))
                    {
                        boardStateManager.PuzzlePieces[i, j] = oldPuzzlePieces[i, j];
                    }

                    // If the previous instance did not contain this index, a new piece needs to be instantiated for it.
                    else
                    {
                        boardStateManager.PuzzlePieces[i, j] = Instantiate(pieceAssets.BlankPiece, boardTransform);

                        SetPieceIndex(boardStateManager.PuzzlePieces[i, j], i, j);
                    }
                }
            }

            // Second loop is through old matrix, to destroy pieces that aren't part of the new instance.
            for (int i = 0; i < oldPuzzlePieces.GetLength(0); i++)
            {
                for (int j = 0; j < oldPuzzlePieces.GetLength(1); j++)
                {
                    // If index is higher than current matrix size, piece needs to be destroyed.
                    if (i >= boardStateManager.PuzzlePieces.GetLength(0) || j >= boardStateManager.PuzzlePieces.GetLength(1))
                    {
                        DestroyImmediate(oldPuzzlePieces[i, j]);
                    }
                }
            }
        }

        /// <summary>
        /// Destroys all puzzle pieces, clearing the puzzle board.
        /// </summary>
        private void DestroyPuzzle()
        {
            for (int i = 0; i < boardStateManager.PuzzlePieces.GetLength(0); i++)
            {
                for (int j = 0; j < boardStateManager.PuzzlePieces.GetLength(1); j++)
                {
                    DestroyImmediate(boardStateManager.PuzzlePieces[i, j]);
                }
            }

            boardStateManager.PuzzlePieces = null;
        }

        /// <summary>
        ///  Sets the pieces' index according to its position in the puzzle matrix.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        private void SetPieceIndex(GameObject piece, int row, int column)
        {
            PieceSwitcher switcher = piece.GetComponent<PieceSwitcher>();
            switcher.Row = row;
            switcher.Column = column;
        }
        #endregion

        #region PREVIEW CREATION
        /// <summary>
        /// Generates a preview for a new puzzle instance according to the selected rows and columns.
        /// Called when a preview instance does not exist yet.
        /// </summary>
        private void CreatePreview()
        {
            // If the preview size is smaller than current puzzle size, preview pieces will be generated for current size.
            // This is because these pieces will be used to visualize pieces that will be deleted.
            int previewRowSize = Mathf.Max(selectedRows, setRows);
            int previewColumnSize = Mathf.Max(selectedColumns, setColumns);

            previewPieces = new GameObject[previewRowSize, previewColumnSize];

            for (int i = 0; i < previewRowSize; i++)
            {
                for (int j = 0; j < previewColumnSize; j++)
                {
                    previewPieces[i, j] = Instantiate(pieceAssets.PreviewPiece, previewTransform);
                }
            }

            // Set the local positions of the preview pieces to display how puzzle will look.
            SetPiecePositions(previewPieces, previewPieces.GetLength(0), previewPieces.GetLength(1));

            // If a puzzle instance exists when preview is generated, its pieces will be aligned to the preview pieces.
            if (boardStateManager.PuzzlePieces != null && boardStateManager.PuzzlePieces.GetLength(0) > 0 && boardStateManager.PuzzlePieces.GetLength(1) > 0)
            {
                MatchPuzzleToPreview();
            }
        }

        /// <summary>
        /// Modifies the preview pieces according to the selected rows and columns.
        /// This method is called when a preview instance already exists.
        /// </summary>
        private void ModifyPreview()
        {
            // We store the old preview matrix in a temporary variable, to delete pieces no longer part of new preview.
            GameObject[,] oldPreview = previewPieces;

            // If the preview size is smaller than current puzzle size, preview pieces will be generated for current size.
            // This is because these pieces will be used to visualize pieces that will be deleted.
            int previewRowSize = Mathf.Max(selectedRows, setRows);
            int previewColumnSize = Mathf.Max(selectedColumns, setColumns);

            previewPieces = new GameObject[previewRowSize, previewColumnSize];

            for (int i = 0; i < previewRowSize; i++)
            {
                for (int j = 0; j < previewColumnSize; j++)
                {
                    // If the old preview has the piece, maintain it, removing the need to instantiate it again.
                    if (i < oldPreview.GetLength(0) && j < oldPreview.GetLength(1))
                    {
                        previewPieces[i, j] = oldPreview[i, j];
                    }

                    // If the new preview needs a piece where it didn't exist, instantiate it.
                    else
                    {
                        previewPieces[i, j] = Instantiate(pieceAssets.PreviewPiece, previewTransform);
                    }
                }
            }

            // Destroy pieces that are no longer part of the new preview.
            for (int i = 0; i < oldPreview.GetLength(0); i++)
            {
                for (int j = 0; j < oldPreview.GetLength(1); j++)
                {
                    if (i >= previewRowSize || j >= previewColumnSize)
                    {
                        DestroyImmediate(oldPreview[i, j]);
                    }
                }
            }

            // Set positions for the updated preview pieces.
            SetPiecePositions(previewPieces, previewPieces.GetLength(0), previewPieces.GetLength(1));

            // If a puzzle instance exists when preview is generated, its pieces will be aligned to the preview pieces.
            if (boardStateManager.PuzzlePieces != null && boardStateManager.PuzzlePieces.GetLength(0) > 0 && boardStateManager.PuzzlePieces.GetLength(1) > 0)
            {
                MatchPuzzleToPreview();
            }
        }

        /// <summary>
        /// Deletes existing preview pieces.
        /// </summary>
        private void DeletePreview()
        {
            for (int i = 0; i < previewPieces.GetLength(0); i++)
            {
                for (int j = 0; j < previewPieces.GetLength(1); j++)
                {
                    DestroyImmediate(previewPieces[i, j]);
                }
            }

            previewPieces = null;
        }

        /// <summary>
        /// Makes the current puzzle instance's piece positions match up with the preview pieces, reflecting how the next instance will look.
        /// Also changes the preview piece's material colors to reflect changes that current row and columns input will inflict.
        /// </summary>
        private void MatchPuzzleToPreview()
        {
            // The puzzle instance's piece positions are set to align with the preview pieces.
            SetPiecePositions(boardStateManager.PuzzlePieces, previewPieces.GetLength(0), previewPieces.GetLength(1));

            // Loop through preview pieces, and set materials to reflect changes.
            for (int i = 0; i < previewPieces.GetLength(0); i++)
            {
                for (int j = 0; j < previewPieces.GetLength(1); j++)
                {
                    // These pieces are of higher or equal index than the selected input, but lower than currently set input, so they will be removed in the next instance.
                    if ((i >= selectedRows && i < setRows) || (j >= selectedColumns && j < setColumns))
                    {
                        previewPieces[i, j].GetComponent<PreviewPieceMeshHandler>().SetRedPreviewMaterial();
                    }

                    // These pieces are of higher index than selected and set input, so they will be added in the next instance.
                    else if (i >= setRows || j >= setColumns)
                    {
                        previewPieces[i, j].GetComponent<PreviewPieceMeshHandler>().SetGreenPreviewMaterial();
                    }

                    // These pieces are unnafected in the next instance.
                    else
                    {
                        previewPieces[i, j].GetComponent<PreviewPieceMeshHandler>().DisableMeshes();
                    }
                }
            }
        }

        /// <summary>
        /// Destroys existing preview pieces, through accessing the preview Transform's children.
        /// Used during initialization, to ensure that preview pieces that manage to persist through scene reloads don't interfere with intended functionality.
        /// This is not for active preview management, where preview matrix is used for operations.
        /// Returns bool so we know whether puzzle pieces need to be repositioned during initialization.
        /// </summary>
        /// <returns></returns>
        private bool DeletePreviewOnInitialization()
        {
            if (previewTransform.childCount > 0)
            {
                for (int i = previewTransform.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(previewTransform.GetChild(i).gameObject);
                }

                return true;
            }

            else
            {
                return false;
            }
        }
        #endregion

        #region CREATION HELPERS
        /// <summary>
        /// Correctly set piece positions for a given puzzle piece matrix.
        /// Is used to set the positions for both the puzzle pieces and the preview pieces.
        /// Target amount parameters signify not the amount of pieces in the matrix, but the amount of entries that should be considered for calculating starting positions.
        /// This means if were setting up the puzzle piece positions to match a preview, target amount should be the preview size and not the puzzle instance's size.
        /// </summary>
        /// <param name="piecesToSet"></param>
        /// <param name="targetRowAmount"></param>
        /// <param name="targetColumnAmount"></param>
        private void SetPiecePositions(GameObject[,] piecesToSet, int targetRowAmount, int targetColumnAmount)
        {
            // NOTE: In the future, there needs to be a way to distinguish if user models are being utilized, and get their size instead.
            PieceBase pieceBase = pieceAssets.BlankPiece.GetComponent<PieceBase>();

            float distanceIncrement = pieceBase.GetPieceSize();

            float startingPositionX = GetGenerationStartingPosition(distanceIncrement, targetColumnAmount);

            float startingPositionY = GetGenerationStartingPosition(distanceIncrement, targetRowAmount);

            for (int i = 0; i < piecesToSet.GetLength(0); i++)
            {
                for (int j = 0; j < piecesToSet.GetLength(1); j++)
                {
                    float positionX = startingPositionX + (distanceIncrement * j);
                    float positionY = startingPositionY + (distanceIncrement * i);

                    piecesToSet[i, j].transform.localPosition = new Vector3(positionX, positionY, 0);
                }
            }
        }

        /// <summary>
        /// Calculates the starting position for a given axis, for generating a puzzle or preview instance.
        /// </summary>
        /// <param name="pieceDistanceIncrement"></param>
        /// <param name="axisTargetAmount"></param>
        /// <returns></returns>
        private float GetGenerationStartingPosition(float pieceDistanceIncrement, int axisTargetAmount)
        {
            float startingPosition = 0;

            // If axis amount is even.
            if (axisTargetAmount % 2 == 0)
            {
                startingPosition += pieceDistanceIncrement / 2;
                startingPosition -= pieceDistanceIncrement * (axisTargetAmount / 2);
            }

            // If axis amount is odd.
            else
            {
                startingPosition -= pieceDistanceIncrement * ((axisTargetAmount - 1) / 2);
            }

            return startingPosition;
        }
        #endregion

        #endregion

        #region PUBLIC METHODS

        #region PUZZLE CREATION
        /// <summary>
        /// Applies changes to the puzzle according to the current row and column input.
        /// Called from custom editor, when user clicks Apply button.
        /// </summary>
        public void ApplyChanges()
        {
            // Puzzle should not be editable during play mode.
            if (Application.isPlaying)
            {
                return;
            }

            // If no changes were made compared to previous puzzle iteration, do nothing.
            if (selectedColumns == setColumns && selectedRows == setRows)
            {
                return;
            }

            // If no puzzle iteration exists, create a new puzzle.
            if (setRows == 0 || setColumns == 0)
            {
                CreatePuzzle();
            }

            // Else, modify the current iteration according to new row and column input.
            else
            {
                ModifyPuzzle();
            }

            // After puzzle instance is generated, the puzzle pieces' local positions need to be set correctly.
            SetPiecePositions(boardStateManager.PuzzlePieces, boardStateManager.PuzzlePieces.GetLength(0), boardStateManager.PuzzlePieces.GetLength(1));

            // setRows and setColumns keep track of the size of the saved puzzle, so their values need to be updated when a new instance is created.
            setRows = selectedRows;
            setColumns = selectedColumns;

            // When a puzzle instance is generated, preview is no longer needed, so delete it.
            DeletePreview();

            // Changes need to be serialized so new puzzle instance is saved.
            EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// Cancels the changes made to row and column fields.
        /// Called from custom editor, when user clicks Cancel button.
        /// </summary>
        public void CancelChanges()
        {
            // Puzzle should not be editable during play mode.
            if (Application.isPlaying)
            {
                return;
            }

            // Only run if user made changes to row or column inputs.
            if (selectedRows == setRows && selectedColumns == setColumns)
            {
                return;
            }

            // Only run if a puzzle instance already exists.
            if (setRows == 0 || setColumns == 0)
            {
                return;
            }

            // Reset selected rows and columns to match saved puzzle instance size.
            selectedRows = setRows;
            selectedColumns = setColumns;

            // If conditions to run CancelChanges() are met, it means a preview for a new puzzle instance exists.
            // When canceling changes, we are resetting to a point where a preview is no longer needed, so we delete it.
            DeletePreview();

            // When a preview is generated, puzzle piece positions are altered to match the preview of the next instance.
            // So, after deleting the preview, we also need to set the puzzle pieces back to their correct position for the current instance.
            SetPiecePositions(boardStateManager.PuzzlePieces, boardStateManager.PuzzlePieces.GetLength(0), boardStateManager.PuzzlePieces.GetLength(1));
        }

        /// <summary>
        /// Clears the current board, deleting all pieces.
        /// </summary>
        public void ClearBoard()
        {
            // Puzzle should not be editable during play mode.
            if (Application.isPlaying)
            {
                return;
            }

            // Only run if a saved puzzle instance exists.
            if (setRows == 0 || setColumns == 0)
            {
                return;
            }

            // Puzzle pieces from current puzzle instance need to be destroyed.
            DestroyPuzzle();

            // Since we no longer have a saved puzzle instance, setRows and setColumns need to be updated to reflect that.
            setRows = 0;
            setColumns = 0;

            // An active preview should always exist when there is no saved puzzle instance.
            CreatePreview();

            // Changes need to be serialized to save the deletion of the puzzle instance.
            EditorUtility.SetDirty(this);
        }
        #endregion

        #region PREVIEW CREATION
        /// <summary>
        /// This method is called when the user changes the row or column input in the custom editor.
        /// It determines whether the preview needs to be generated, modified or deleted.
        /// Upon determining this, it will call the corresponding method.
        /// </summary>
        public void DeterminePreviewAdjustments()
        {
            // We only want this to run in edit mode.
            if (Application.isPlaying)
            {
                return;
            }

            // When selected rows and columns are the same as the set rows and columns, we want to delete the preview (if it exists).
            if (selectedRows == setRows && selectedColumns == setColumns)
            {
                if (previewPieces != null)
                {
                    DeletePreview();

                    // Since before preview deletion puzzle pieces were matching preview's position, we reset its position to be accurate to its previewless instance.
                    if (boardStateManager.PuzzlePieces != null)
                    {
                        SetPiecePositions(boardStateManager.PuzzlePieces, boardStateManager.PuzzlePieces.GetLength(0), boardStateManager.PuzzlePieces.GetLength(1));
                    }
                }
            }

            // When selected rows and columns differ from the set rows and columns, we want a preview.
            else
            {
                // If there is no preview, we want to generate a new one.
                if (previewPieces == null)
                {
                    CreatePreview();
                }

                // If there is a preview, we want to modify it.
                else
                {
                    ModifyPreview();
                }
            }
        }
        #endregion

        #region LIMITER VALUES
        /// <summary>
        /// Switches the puzzle's size limiter on or off, ensuring restrictions are in place to not enable it with invalid limiter values.
        /// Is called from the custom editor.
        /// </summary>
        /// <param name="desiredState"></param>
        public void SetLimiterState(bool desiredState)
        {
            // When trying to enable the limiter, we need to make sure it won't interfere with current puzzle instance's values.
            if (desiredState == true)
            {
                // Can't enable limiter if its value is lower than current puzzle instance's row or column values.
                if (limiterValue < setRows || limiterValue < setColumns)
                {
                    Debug.LogWarning("Can't enable limiter because value is lower than current puzzle instance's row or column values");
                    return;
                }

                // Can't enable limiter if its value is lower than currently selected row or column values.
                if (limiterValue < selectedRows || limiterValue < selectedColumns)
                {
                    Debug.LogWarning("Can't enable limiter because value is lower than currently selected row or column values");
                    return;
                }
            }

            isLimited = desiredState;

            EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// Sets the puzzle's size limiter's value, which determines how many puzzle pieces can be created for its rows and columns.
        /// Is called from the custom editor.
        /// </summary>
        /// <param name="desiredValue"></param>
        public void SetLimiterValue(int desiredValue)
        {
            // When the limiter is enabled, values need to be clamped according to current puzzle instance to avoid errors.
            if (isLimited)
            {
                // Limiter value can't be lower than current puzzle instance's row or columns values.
                if (desiredValue < setRows || desiredValue < setColumns)
                {
                    Debug.LogWarning("Can't set limiter value lower than current puzzle instance's axis values.");
                    return;
                }

                // Limiter value can't be lower than the currently selected row or column values.
                if (desiredValue < selectedRows || desiredValue < selectedColumns)
                {
                    Debug.LogWarning("Can't set limiter value lower than current row or column selection.");
                    return;
                }
            }

            if (desiredValue < 1)
            {
                limiterValue = 1;
            }

            else
            {
                limiterValue = desiredValue;
            }

            EditorUtility.SetDirty(this);
        }
        #endregion

        #endregion
    }
}