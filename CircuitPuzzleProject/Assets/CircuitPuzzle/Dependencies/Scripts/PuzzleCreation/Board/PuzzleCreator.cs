using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CircuitPuzzle
{
    [ExecuteInEditMode]
    public class PuzzleCreator : MonoBehaviour, ISerializationCallbackReceiver
    {
        #region FIELDS
        // References to assets to be used for custom inspector.
        private SOAssetHolder references;

        // Reference to transform where piece prefabs will be instantiated.
        private Transform boardTransform;

        // The number of rows and columns the user currently has inputted in the inspector.
        [SerializeField, HideInInspector]
        private int selectedRows;
        [SerializeField, HideInInspector]
        private int selectedColumns;

        // The number of rows and columns that the last created puzzle iteration contains.
        [SerializeField, HideInInspector]
        private int setRows;
        [SerializeField, HideInInspector]
        private int setColumns;

        // Reference to the puzzle piece prefab.
        private GameObject blankPiece;

        // Matrix that contains the gameobject prefab of each individual puzzle piece.
        private GameObject[,] puzzlePieces;

        // List the matrix will be converted to so it can be serialized.
        [SerializeField, HideInInspector]
        private List<PuzzlePackage<GameObject>> serializablePieces;

        // Struct with info to convert the matrix to and from the list.
        [System.Serializable]
        private struct PuzzlePackage<TElement>
        {
            public int Row;
            public int Column;
            public TElement Element;

            public PuzzlePackage(int row, int column, TElement element)
            {
                Row = row;
                Column = column;
                Element = element;
            }
        }

        // References to the MeshRenderer and SpriteRenderer rendering the puzzle piece's model and sprite respectively.
        private MeshRenderer pieceMeshRenderer;

        // Boolean to limit the number of pieces allowed for rows and columns.
        [SerializeField, HideInInspector]
        private bool isLimited = true;

        // Boolean that determines whether the creation of the puzzle was cleared from undo stack.
        [SerializeField, HideInInspector]
        private bool undoCleared;

        // Integers to keep track of whether preview rows and columns need to be added or removed.
        [SerializeField, HideInInspector]
        private int previewRows;
        [SerializeField, HideInInspector]
        private int previewColumns;

        // Matrix containing preview pieces.
        private GameObject[,] previewPieces;

        // Reference to preview piece prefabs.
        private GameObject previewPiece;

        // Transform where preview pieces will be instantiated.
        private Transform previewTransform;

        // Materials for preview pieces.
        private Material greenBase;
        private Material redBase;

        // List containing the preview matrix.
        [SerializeField, HideInInspector]
        private List<PreviewPackage<GameObject>> serializablePreview;

        // Struct with info to convert the preview matrix to and from the list.
        [System.Serializable]
        private struct PreviewPackage<TElement>
        {
            public int Row;
            public int Column;
            public TElement Element;

            public PreviewPackage(int row, int column, TElement element)
            {
                Row = row;
                Column = column;
                Element = element;
            }
        }
        #endregion

        #region PROPERTIES
        // Assures that the number of rows does does not go below 0 or above 100.
        public int SelectedRows
        {
            get { return selectedRows; }
            set
            {
                if (value < 1)
                {
                    selectedRows = 1;
                }
                else if (value > 20 && isLimited)
                {
                    selectedRows = 20;
                }
                else
                {
                    selectedRows = value;
                }
            }
        }
        // Same as the property for rows.
        public int SelectedColumns
        {
            get { return selectedColumns; }
            set
            {
                if (value < 1)
                {
                    selectedColumns = 1;
                }
                else if (value > 20 && isLimited)
                {
                    selectedColumns = 20;
                }
                else
                {
                    selectedColumns = value;
                }
            }
        }
        public SOAssetHolder References { get => references; private set => references = value; }
        public int SetRows { get => setRows; private set => setRows = value; }
        public int SetColumns { get => setColumns; private set => setColumns = value; }
        public bool IsLimited { get => isLimited; set => isLimited = value; }
        public GameObject[,] PuzzlePieces { get => puzzlePieces; set => puzzlePieces = value; }
        public bool UndoCleared { get => undoCleared; set => undoCleared = value; }
        public int PreviewRows { get => previewRows; private set => previewRows = value; }
        public int PreviewColumns { get => previewColumns; private set => previewColumns = value; }
        public GameObject[,] PreviewPieces { get => previewPieces; private set => previewPieces = value; }
        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            // Get assetReferences object.
            references = GetComponent<SOAssetHolder>();

            // Get board tranform reference.
            boardTransform = transform.GetChild(0);

            // Get preview transform reference.
            previewTransform = transform.GetChild(1);

            // Get piece prefab from assets.
            blankPiece = references.PuzzleCreatorAssets.BlankPiecePrefab;

            // Preview pieces prefab references.
            previewPiece = references.PuzzleCreatorAssets.PreviewPiecePrefab;

            // Get MeshRenderer and SpriteRenderer references.
            pieceMeshRenderer = blankPiece.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>();

            // Get material references.
            greenBase = references.PuzzleCreatorAssets.GreenBase;
            redBase = references.PuzzleCreatorAssets.RedBase;

            // If puzzle matrix has not been initialized, do so.
            if (puzzlePieces == null)
            {
                puzzlePieces = new GameObject[0, 0];
            }
        }
        #endregion

        #region PUBLIC METHODS
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

            // Changes will only be applied if user changed row or column input.
            if (selectedColumns != setColumns || selectedRows != setRows)
            {
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
                ResetPreview();

                // Mark scene as dirty so hierarchy changes can be saved.
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
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
            // Will also only run if a puzzle iteration already exists.
            if ((selectedColumns != setColumns || selectedRows != setRows) && setColumns != 0 && SetRows != 0)
            {
                selectedRows = setRows;
                selectedColumns = setColumns;

                // Mark scene as dirty so hierarchy changes can be saved.
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
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
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        /// <summary>
        /// Generates the preview for the next puzzle instance according to selected row and column input.
        /// </summary>
        public void GeneratePreview()
        {
            // We only want this to run in edit mode.
            if (Application.isPlaying)
            {
                return;
            }

            // Set previous row and column.
            previewRows = selectedRows;
            previewColumns = selectedColumns;

            // If a preview already exists, delete it.
            if (previewPieces != null)
            {
                DeletePreviewPieces();
            }

            // Get size for preview matrix.
            int matrixRows = GetBiggerValue(selectedRows, SetRows);
            int matrixColumns = GetBiggerValue(selectedColumns, setColumns);


            // Initialize matrix that will contain new preview.
            previewPieces = new GameObject[matrixRows, matrixColumns];

            // Populate matrix.
            CreatePreviewPieces();

            // Set preview position.
            SetPiecePositions(previewPieces, previewPieces.GetLength(0), previewPieces.GetLength(1));

            // Set piece positions to match preview.
            if (puzzlePieces.GetLength(0) > 0 && puzzlePieces.GetLength(1) > 0)
            {
                MatchPuzzleToPreview();

                SetPreviewMaterials();
            }

            // Mark scene as dirty.
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        /// <summary>
        /// Deletes all preview piece gameobjects and sets the matrix that contained them as null.
        /// </summary>
        public void ResetPreview()
        {
            // We only want this to run in edit mode.
            if (Application.isPlaying)
            {
                return;
            }

            // Delete preview piece gameobjects.
            DeletePreviewPieces();

            // Set preview matrix to null.
            previewPieces = new GameObject[0, 0];

            // Mark scene as dirty.
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        #endregion

        #region SERIALIZATION
        /// <summary>
        /// Converts the puzzle matrix to a list and serializes it.
        /// Does the same with the preview matrix.
        /// </summary>
        public void OnBeforeSerialize()
        {
            // Puzzle matrix.
            serializablePieces = new List<PuzzlePackage<GameObject>>();
            for (int i = 0; i < puzzlePieces.GetLength(0); i++)
            {
                for (int j = 0; j < puzzlePieces.GetLength(1); j++)
                {
                    serializablePieces.Add(new PuzzlePackage<GameObject>(i, j, puzzlePieces[i, j]));
                }
            }

            // Preview matrix.
            if(previewPieces.GetLength(0) > 0 && previewPieces.GetLength(1) > 0)
            {
                serializablePreview = new List<PreviewPackage<GameObject>>();
                for (int i = 0; i < previewPieces.GetLength(0); i++)
                {
                    for (int j = 0; j < previewPieces.GetLength(1); j++)
                    {
                        serializablePreview.Add(new PreviewPackage<GameObject>(i, j, previewPieces[i, j]));
                    }
                }
            }
        }

        /// <summary>
        /// Converts the serialized list back into the puzzle matrix.
        /// Does the same with the preview matrix.
        /// </summary>
        public void OnAfterDeserialize()
        {
            // Puzzle matrix.
            puzzlePieces = new GameObject[setRows, setColumns];

            if (puzzlePieces.GetLength(0) > 0 && puzzlePieces.GetLength(1) > 0)
            {
                foreach (var package in serializablePieces)
                {
                    puzzlePieces[package.Row, package.Column] = package.Element;
                }
            }

            // Preview matrix.
            int matrixRows = GetBiggerValue(selectedRows, SetRows);
            int matrixColumns = GetBiggerValue(selectedColumns, SetColumns);

            previewPieces = new GameObject[matrixRows, matrixColumns];

            if (previewPieces.GetLength(0) > 0 && previewPieces.GetLength(1) > 0)
            {
                foreach (var package in serializablePreview)
                {
                    previewPieces[package.Row, package.Column] = package.Element;
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
                    pieces[i, j] = Instantiate(blankPiece, boardTransform);

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
                        newPieces[i, j] = Instantiate(blankPiece, boardTransform);

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
        /// Sets the position of each individual piece prefab on the board.
        /// Piece positions are properly aligned according to row and column input, as well as the size of its model or sprite.
        /// </summary>
        /// <param name="pieces"></param>
        private void SetPiecePositions(GameObject[,] pieces, int rows, int columns)
        {
            // Value that will be used to increment the position of each puzzle piece.
            float increment = 0;

            // Set the increment according to the model's width.
            increment = pieceMeshRenderer.bounds.size.x;

            // Get starting position for the X axis.
            float startingPositionX = 0;
            // If X axis elements are even.
            if (columns % 2 == 0)
            {
                startingPositionX += increment / 2;
                startingPositionX -= increment * ((columns) / 2);
            }
            // If X axis elements are odd.
            else
            {
                startingPositionX -= increment * ((columns - 1) / 2);
            }

            // Get starting position for the Y axis.
            float startingPositionY = 0;
            // If Y axis elements are even.
            if (rows % 2 == 0)
            {
                startingPositionY += increment / 2;
                startingPositionY -= increment * (rows / 2);
            }
            // If Y axis elements are odd.
            else
            {
                startingPositionY -= increment * ((rows - 1) / 2);
            }

            // Loop through matrix.
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    // Gets the X and Y position for the current puzzle piece.
                    float positionX = startingPositionX + (increment * j);
                    float positionY = startingPositionY + (increment * i);

                    // Sets the piece's position.
                    pieces[i, j].transform.localPosition = new Vector3(positionX, positionY, 0);
                    // Sets the piece's name the same as its index in the matrix.
                    pieces[i, j].name = "[" + i + ", " + j + "]";

                    if (j + 1 == pieces.GetLength(1))
                    {
                        break;
                    }
                }

                if (i + 1 == pieces.GetLength(0))
                {
                    break;
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

        #region PREVIEW
        /// <summary>
        /// Takes in 2 values and returns the biggest one.
        /// </summary>
        /// <param name="valueOne"></param>
        /// <param name="valueTwo"></param>
        /// <returns></returns>
        private int GetBiggerValue(int valueOne, int valueTwo)
        {
            if (valueOne > valueTwo)
            {
                return valueOne;
            }

            else
            {
                return valueTwo;
            }
        }

        /// <summary>
        /// Populates the preview matrix by instantiating preview piece prefabs into it.
        /// </summary>
        private void CreatePreviewPieces()
        {
            for (int i = 0; i < previewPieces.GetLength(0); i++)
            {
                for (int j = 0; j < previewPieces.GetLength(1); j++)
                {
                    previewPieces[i, j] = Instantiate(previewPiece, previewTransform);
                }
            }
        }

        /// <summary>
        /// Deletes the preview piece prefabs from the preview matrix.
        /// </summary>
        private void DeletePreviewPieces()
        {
            for (int i = 0; i < previewPieces.GetLength(0); i++)
            {
                for (int j = 0; j < previewPieces.GetLength(1); j++)
                {
                    DestroyImmediate(previewPieces[i, j]);
                }
            }
        }

        /// <summary>
        /// Adjusts this puzzle iteration piece positions to match the positions of the preview pieces.
        /// </summary>
        private void MatchPuzzleToPreview()
        {
            for (int i = 0; i < puzzlePieces.GetLength(0); i++)
            {
                for (int j = 0; j < puzzlePieces.GetLength(1); j++)
                {
                    puzzlePieces[i, j].transform.localPosition = previewPieces[i, j].transform.localPosition;
                }
            }
        }

        /// <summary>
        /// Sets the material of the preview pieces according to what will happen in the next puzzle iteration.
        /// Pieces that are to be added will be set to green.
        /// Pieces that are to be removed will be set to red.
        /// Pieces that are not affected will have their meshes disabled.
        /// </summary>
        private void SetPreviewMaterials()
        {
            for (int i = 0; i < previewPieces.GetLength(0); i++)
            {
                for (int j = 0; j < previewPieces.GetLength(1); j++)
                {
                    // If pieces are to be added
                    if ((i >= puzzlePieces.GetLength(0) && j <= selectedColumns - 1) || (j >= puzzlePieces.GetLength(1) && i <= selectedRows - 1))
                    {
                        MeshRenderer[] renderers = GetPreviewMeshes(previewPieces[i, j]);
                        renderers[0].material = greenBase;
                    }

                    // If pieces are to be removed.
                    else if ((i >= selectedRows && i < setRows) || (j >= selectedColumns && j < setColumns))
                    {
                        MeshRenderer[] renderers = GetPreviewMeshes(previewPieces[i, j]);
                        renderers[0].material = redBase;
                    }

                    // If pieces are unnafected.
                    else
                    {
                        MeshRenderer[] renderers = GetPreviewMeshes(previewPieces[i, j]);
                        renderers[0].enabled = false;
                        renderers[1].enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Get Meshrenderer references from preview pieces.
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        private MeshRenderer[] GetPreviewMeshes(GameObject go)
        {
            MeshRenderer[] renderers = new MeshRenderer[2];

            renderers[0] = go.GetComponent<MeshRenderer>();
            renderers[1] = go.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();

            return renderers;
        }
        #endregion
        #endregion
    }
}