using UnityEngine;
using UnityEditor;

namespace CircuitPuzzle
{
    [ExecuteInEditMode]
    public class PuzzleCreator : MonoBehaviour
    {
        #region FIELDS
        // The number of rows and columns that the last created puzzle iteration contains.
        [SerializeField]
        private int setRows;
        [SerializeField]
        private int setColumns;
        // Defines the maximum number of puzzle pieces per axis when limiter is enabled.
        [SerializeField]
        private int limiterValue;
        // Boolean to limit the number of pieces allowed for rows and columns.
        [SerializeField]
        private bool isLimited;

        // The number of rows and columns the user currently has inputted in the inspector, in the custom editor.
        private int selectedRows;
        private int selectedColumns;

        // Reference to transform that will serve as parent to instantiated puzzle pieces.
        private Transform boardTransform;
        // Transform where preview pieces will be instantiated.
        private Transform previewTransform;
        // Matrix that contains the reference to the gameobject prefab of each individual puzzle piece in the puzzle.
        // The piece's position in the matrix is the same as its position in the puzzle.
        private GameObject[,] puzzlePieces;
        // Matrix containing preview pieces.
        private GameObject[,] previewPieces;

        // This holds references to the prefabs used to instantiate puzzle pieces.
        private PieceAssetsSO pieceAssets;

        // These provide the string representation of this class' fields to the custom inspector, to access their SerializedProperty.
        public const string limiterValueName = nameof(limiterValue);
        public const string isLimitedName = nameof(isLimited);
        #endregion

        #region PROPERTIES
        public int SelectedRows
        {
            get { return selectedRows; }
            set
            {
                // A value below 1 wouldn't allow a puzzle to be created.
                if (value < 1)
                {
                    selectedRows = 1;
                }
                // Limit value when limiter is enabled.
                else if (value > limiterValue && isLimited)
                {
                    selectedRows = limiterValue;
                }
                else
                {
                    selectedRows = value;
                }
            }
        }

        public int SelectedColumns
        {
            get { return selectedColumns; }
            set
            {
                // A value below 1 wouldn't allow a puzzle to be created.
                if (value < 1)
                {
                    selectedColumns = 1;
                }
                // Limit value when limiter is enabled.
                else if (value > limiterValue && isLimited)
                {
                    selectedColumns = limiterValue;
                }
                else
                {
                    selectedColumns = value;
                }
            }
        }

        public int SetRows { get => setRows; private set => setRows = value; }
        public int SetColumns { get => setColumns; private set => setColumns = value; }
        public int LimiterValue { get => limiterValue; private set => limiterValue = value; }
        public bool IsLimited { get => isLimited; private set => isLimited = value; }
        public GameObject[,] PuzzlePieces { get => puzzlePieces; private set => puzzlePieces = value; }
        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            pieceAssets = GetComponent<PuzzleAssetsHolder>().PieceAssets;

            boardTransform = transform.GetChild(0);

            previewTransform = transform.GetChild(1);

            // If boardTransform has no children, it means there is no instantiated puzzle instance.
            // Values for selected rows and columns need to be reset to default value, and an initial preview needs to be generated.
            if (boardTransform.childCount == 0)
            {
                selectedRows = 1;
                selectedColumns = 1;

                // If user saved scene containing a PuzzleCreator with no instantiated puzzle instance, preview pieces will be saved in the scene.
                // Preview pieces are not meant to be persistent (and previewPieces matrix is not serialized).
                // So, existing preview pieces on scene load need to be deleted, before generating new preview for the default values.
                DeletePreviewOnInitialization();

                GeneratePreview();
            }

            // If boardTransform has children, there is an instantiated puzzle instance.
            // Values for selected rows and columns needs to be equal to instance's set rows and columns.
            // Additionally, puzzlePieces matrix needs to be populated with existing puzzle pieces.
            else
            {
                selectedRows = setRows;
                selectedColumns = setColumns;

                // setRows and setColumns are serialized and will accurately tell us the size of the saved puzzle between scene reloads.
                // Due to this, we can use it to initialize the puzzlePieces matrix.
                puzzlePieces = new GameObject[setRows, setColumns];

                // When creating or modifying a puzzle, the pieces themselves are fed their position in the matrix.
                // We can retrieve this info from them, to repopulate the matrix on initialization.
                for (int i = 0; i < boardTransform.childCount; i++)
                {
                    PieceSwitcher switcher = boardTransform.GetChild(i).GetComponent<PieceSwitcher>();

                    puzzlePieces[switcher.Row, switcher.Column] = switcher.gameObject;
                }

                // When a puzzle instance exists on scene load, there should not be a preview until the user changes setRows or setColumns value.
                // If preview pieces were saved to the scene due to an unrelated action making the scene dirty, they need to be deleted.
                if (DeletePreviewOnInitialization())
                {
                    // When a preview exists, the puzzle instance's piece's position is modified to match said preview.
                    // So if preview pieces did exist on the scene and were deleted, puzzle piece transforms need to be set to the correct previewless position.
                    SetPiecePositions(puzzlePieces, puzzlePieces.GetLength(0), puzzlePieces.GetLength(1));
                }
            }
        }
        #endregion

        #region PRIVATE METHODS

        #region PUZZLE CREATION
        /// <summary>
        /// Creates the puzzle board by populating puzzle matrix with individual piece prefabs.
        /// This creates the first iteration of this instance's puzzle, with default puzzle piece prefabs only.
        /// </summary>
        /// <returns>Matrix containing the gameobjects for the puzzle's pieces</returns>
        private GameObject[,] CreatePuzzle()
        {
            // Create container for new puzzle.
            GameObject[,] pieces = new GameObject[selectedRows, selectedColumns];

            // Loop through matrix.
            for (int i = 0; i < pieces.GetLength(0); i++)
            {
                for (int j = 0; j < pieces.GetLength(1); j++)
                {
                    // Fill matrix with blank piece prefabs.
                    pieces[i, j] = Instantiate(pieceAssets.BlankPiece, boardTransform);

                    // Feed the piece it's own position in the matrix so it can be switched later.
                    SetPieceIndex(pieces[i, j], i, j);
                }
            }
            // Return the piece matrix.
            return pieces;
        }

        /// <summary>
        /// Creates the puzzle board by populating puzzle matrix with individual piece prefabs.
        /// This overload creates any iteration of the puzzle after the first one.
        /// Either new pieces are added to already existing puzzle, or pieces are removed from it.
        /// Changes made to piece prefabs belonging to the previous puzzle iteration are not affected.
        /// </summary>
        /// <param name="oldPieces"></param>
        /// <returns>Matrix containing the gameobjects for the puzzle's pieces</returns>
        private GameObject[,] CreatePuzzle(GameObject[,] oldPieces)
        {
            // Create container for newly created puzzle.
            GameObject[,] newPieces = new GameObject[selectedRows, selectedColumns];

            // Loop through matrix.
            for (int i = 0; i < selectedRows; i++)
            {
                for (int j = 0; j < selectedColumns; j++)
                {
                    // If previous iteration contains this element.
                    if (i < oldPieces.GetLength(0) && j < oldPieces.GetLength(1))
                    {
                        // Piece remains the same.
                        newPieces[i, j] = oldPieces[i, j];
                    }

                    // If previous iteration does not contain this element.
                    else
                    {
                        // New instance is created.
                        newPieces[i, j] = Instantiate(pieceAssets.BlankPiece, boardTransform);

                        // Set piece index.
                        SetPieceIndex(newPieces[i, j], i, j);
                    }
                }
            }

            // Return the matrix with the newly created puzzle pieces.
            return newPieces;
        }

        /// <summary>
        ///  Sets the pieces index according to its position in the puzzle matrix.
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

        /// <summary>
        /// This method will remove rows and columns that were in the previous puzzle iteration but not on the new one.
        /// </summary>
        private void DeleteRemovedPieces(GameObject[,] oldPieces)
        {
            // Loop through matrix.
            for (int i = 0; i < oldPieces.GetLength(0); i++)
            {
                for (int j = 0; j < oldPieces.GetLength(1); j++)
                {
                    // If old iteration contains this element but new one does not.
                    if (i >= puzzlePieces.GetLength(0) || j >= puzzlePieces.GetLength(1))
                    {
                        // Destroy piece GameObject.
                        DestroyImmediate(oldPieces[i, j]);
                    }
                }
            }
        }

        /// <summary>
        /// Deletes all the pieces from the puzzle matrix and makes it into a an empty matrix of 0 length.
        /// </summary>
        /// <param name="pieces"></param>
        private void DeleteBoard()
        {
            // Loop through matrix.
            for (int i = 0; i < puzzlePieces.GetLength(0); i++)
            {
                for (int j = 0; j < puzzlePieces.GetLength(1); j++)
                {
                    // Destroy the gameobject for each piece.
                    DestroyImmediate(puzzlePieces[i, j]);
                }
            }

            // Reset the matrix.
            puzzlePieces = new GameObject[0, 0];
        }
        #endregion

        #region PREVIEW CREATION
        /// <summary>
        /// Generates a preview of the puzzle pieces according to the selected rows and columns.
        /// This method is called when a preview instance does not exist yet.
        /// </summary>
        private void GeneratePreview()
        {
            int previewRowSize = Mathf.Max(selectedRows, setRows);
            int previewColumnSize = Mathf.Max(selectedColumns, setColumns);

            // Since this is a fresh preview, previewPieces needs to be initialized.
            previewPieces = new GameObject[previewRowSize, previewColumnSize];

            // Loop through the selected rows and columns to instantiate preview pieces.
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
            if (puzzlePieces != null && puzzlePieces.GetLength(0) > 0 && puzzlePieces.GetLength(1) > 0)
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
            // We store the old preview pieces in a temporary variable, to determine how to handle the new preview.
            GameObject[,] oldPreview = previewPieces;

            int previewRowSize = Mathf.Max(selectedRows, SetRows);
            int previewColumnSize = Mathf.Max(selectedColumns, setColumns);

            // Reinitialize the preview matrix to the new selected rows and columns.
            previewPieces = new GameObject[previewRowSize, previewColumnSize];

            // Loop through the selected rows and columns to instantiate preview pieces.
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
            if (puzzlePieces != null && puzzlePieces.GetLength(0) > 0 && puzzlePieces.GetLength(1) > 0)
            {
                MatchPuzzleToPreview();
            }
        }

        /// <summary>
        /// Deletes the existing preview pieces.
        /// </summary>
        private void DeletePreview()
        {
            // Loop through the preview matrix to destroy all preview pieces.
            for (int i = 0; i < previewPieces.GetLength(0); i++)
            {
                for (int j = 0; j < previewPieces.GetLength(1); j++)
                {
                    DestroyImmediate(previewPieces[i, j]);
                }
            }

            // Reset the preview matrix.
            previewPieces = null;
        }

        /// <summary>
        /// Makes the current puzzle instance's piece positions match up with the preview pieces, reflecting how the next instance will look.
        /// Also changes the preview piece's material colors to reflect changes that current row and columns input will inflict.
        /// </summary>
        private void MatchPuzzleToPreview()
        {
            // The puzzle instance's piece positions are set to align with the preview pieces.
            SetPiecePositions(puzzlePieces, previewPieces.GetLength(0), previewPieces.GetLength(1));

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

        #region GENERAL
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

            // Set the distance increment according to the model's width.
            float distanceIncrement = pieceBase.GetPieceSize();

            // Get starting position for the X axis.
            float startingPositionX = GetGenerationStartingPosition(distanceIncrement, targetColumnAmount);

            // Get starting position for the Y axis.
            float startingPositionY = GetGenerationStartingPosition(distanceIncrement, targetRowAmount);

            // Loop through matrix.
            for (int i = 0; i < piecesToSet.GetLength(0); i++)
            {
                for (int j = 0; j < piecesToSet.GetLength(1); j++)
                {
                    // Gets the X and Y position for the current puzzle piece.
                    float positionX = startingPositionX + (distanceIncrement * j);
                    float positionY = startingPositionY + (distanceIncrement * i);

                    // Sets the piece's position.
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
        /// </summary>
        public void ApplyChanges()
        {
            // We only want this to run in edit mode.
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
            if (SetRows == 0 || setColumns == 0)
            {
                puzzlePieces = CreatePuzzle();
            }

            // Else, modify the current iteration according to new row and column input.
            else
            {
                // Container for previous puzzle iteration, will be used to delete removed rows or columns.
                GameObject[,] oldPieces = puzzlePieces;

                // Create a new puzzle iteration, keeping unchanged pieces from previous iteration.
                puzzlePieces = CreatePuzzle(oldPieces);

                // Delete pieces from previous iteration that were removed in new iteration.
                DeleteRemovedPieces(oldPieces);
            }

            // Set the local positions of the puzzle pieces inside the puzzle matrix.
            SetPiecePositions(puzzlePieces, puzzlePieces.GetLength(0), puzzlePieces.GetLength(1));

            // Adjust setRows and setColumns value to match changes.
            setRows = selectedRows;
            setColumns = selectedColumns;

            // Delete preview after creating new iteration.
            DeletePreview();

            // Mark scene as dirty so hierarchy changes can be saved.
            EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// Cancels the changes made to row and column inputs.
        /// </summary>
        public void CancelChanges()
        {
            // We only want this to run in edit mode.
            if (Application.isPlaying)
            {
                return;
            }

            // Function will only run if user made changes to row or column inputs.
            if (selectedRows == setRows && selectedColumns == setColumns)
            {
                return;
            }

            // Will also only run if a puzzle iteration already exists.
            if (setRows == 0 || setColumns == 0)
            {
                return;
            }

            // Reset selected rows and columns to match the last puzzle iteration.
            selectedRows = setRows;
            selectedColumns = setColumns;

            // Since row and column input is reset, preview needs to be deleted, and piece positions need to be reset to current instance's input.
            DeletePreview();
            SetPiecePositions(puzzlePieces, puzzlePieces.GetLength(0), puzzlePieces.GetLength(1));
        }

        /// <summary>
        /// Clears the current board, deleting all pieces.
        /// </summary>
        public void ClearBoard()
        {
            // We only want this to run in edit mode.
            if (Application.isPlaying)
            {
                return;
            }

            // Delete the board.
            DeleteBoard();

            // Reset the setRows and setColumns variables.
            setRows = 0;
            SetColumns = 0;

            // Generate preview after clearing board.
            GeneratePreview();

            // Mark scene as dirty so changes can be saved.
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
                    if (puzzlePieces != null)
                    {
                        SetPiecePositions(puzzlePieces, puzzlePieces.GetLength(0), puzzlePieces.GetLength(1));
                    }
                }
            }

            // When selected rows and columns differ from the set rows and columns, we want a preview.
            else
            {
                // If there is no preview, we want to generate a new one.
                if (previewPieces == null)
                {
                    GeneratePreview();
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