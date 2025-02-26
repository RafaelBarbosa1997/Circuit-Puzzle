using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

namespace CircuitPuzzle
{
    public class PowerManager : MonoBehaviour
    {
        #region FIELDS
        // Reference to the puzzle manager this piece belongs to.
        private PuzzleManager puzzleManager;

        // Reference to puzzle settings to check puzzle type.
        private PuzzleSettings puzzleSettings;

        // Reference to user model selection.
        private UserModelSelection userModelSelection;

        // Reference to piece base to get type index.
        private PieceBase pieceBase;

        // Reference to model container to change model to reflect power status.
        private ModelContainer modelContainer;

        // Transforms for switching models.
        private Transform defaultModelTransform;
        private Transform customModelTransform;

        // Reference to interact point, used to end interaction on completion of one time completion type puzzles.
        private InteractPoint interactPoint;

        // Boolean that keeps track of whether this piece is powered or not.
        [SerializeField]
        private bool isPowered;

        // Keeps track of the wires that check for connections.
        [SerializeField]
        private ConnectionManager[] wires;

        // List containing the PowerManager component of connected pieces.
        private List<PowerManager> connectedPieces;

        // Boolean to determine whether an ending piece's power status changed compared to previous puzzle state.
        private bool wasPreviouslyPowered;
        #endregion

        #region PROPERTIES
        public bool IsPowered { get => isPowered; set => isPowered = value; }
        public bool WasPreviouslyPowered { get => wasPreviouslyPowered; set => wasPreviouslyPowered = value; }
        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            // Initialize connected pieces list.
            connectedPieces = new List<PowerManager>();

            // Get PuzzleManager reference.
            Transform pieceParent = transform.parent;
            Transform boardParent = pieceParent.parent;
            puzzleManager = boardParent.gameObject.GetComponent<PuzzleManager>();

            // Get PuzzleSettings reference.
            puzzleSettings = boardParent.GetComponent<PuzzleSettings>();

            // Get InteractPoint reference.
            Transform interactTransform = boardParent.GetChild(2);
            interactPoint = interactTransform.GetComponent<InteractPoint>();

            // Get piece base reference.
            pieceBase = GetComponent<PieceBase>();

            // Get Model Container reference.
            modelContainer = boardParent.gameObject.GetComponent<ModelContainer>();

            // Get user model selection.
            userModelSelection = boardParent.gameObject.GetComponent<UserModelSelection>();

            // Get models transforms.
            defaultModelTransform = transform.GetChild(0);
            customModelTransform = transform.GetChild(2);
        }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Method used to check if this piece is connected to any others.
        /// This method is used on starting pieces to begin to check the puzzle's power status.
        /// Any connected piece will have its power turned on and will run the overload for this method on any connected pieces, recursively.
        /// </summary>
        public void CheckConnections()
        {
            // First clear the connected pieces list.
            connectedPieces.Clear();

            // Next, fill the connected pieces list by checking for connected wires.
            for(int i = 0; i < wires.Length; i++)
            {
                GetConnectedPiece(i);
            }
            
            // Turn on the power of connected pieces and the pieces connected to them, recursively.
            foreach(PowerManager manager in connectedPieces)
            {
                manager.CheckConnections(this);
            }
        }

        /// <summary>
        /// Overload for the CheckConnections method.
        /// This is not called directly and is instead called by other connected pieces using this same method, recursively.
        /// </summary>
        /// <param name="previousConnection"></param>
        public void CheckConnections(PowerManager previousConnection)
        {
            // Turn on power for this piece.
            isPowered = true;

            // Set model.
            // Default model.
            if(defaultModelTransform.childCount > 0)
            {
                // Delete current model.
                foreach(Transform child in defaultModelTransform)
                {
                    Destroy(child.gameObject);
                }

                // Instantiate new model.
                Instantiate(modelContainer.DefaultPoweredModels[pieceBase.TypeIndex], defaultModelTransform);
            }

            // User model.
            if(customModelTransform.childCount > 0)
            {
                // Delete current model.
                foreach(Transform child in customModelTransform)
                {
                    Destroy(child.gameObject);
                }

                // Instantiate new model.
                Instantiate(modelContainer.CustomPoweredModels[pieceBase.TypeIndex], customModelTransform);
            }

            // Clear connected pieces list.
            connectedPieces.Clear();

            // Fill connected pieces list by checking for connected wires.
            for(int i = 0; i < wires.Length; i++)
            {
                GetConnectedPiece(i);
            }

            bool isLastPiece = true;

            // Turn on power of connected pieces and pieces connected to them, recursively.
            foreach(PowerManager manager in connectedPieces)
            {
                // If piece were checking is same piece that powered our current piece, ignore it as it's already been powered.
                if(manager != previousConnection)
                {
                    manager.CheckConnections(this);

                    isLastPiece = false;
                }
            }

            // If this piece isn't connected to any others, check if any ending pieces were powered off.
            // If so, run their respective OnPoweredOff events.
            if (isLastPiece)
            {
                puzzleManager.SwitchPiecePower();
            }
        }

        /// <summary>
        /// Reset the wire model back to default material.
        /// </summary>
        public void ResetWireColor()
        {
            // Default model.
            if(defaultModelTransform.childCount > 0)
            {
                // Destroy current model.
                foreach(Transform child in defaultModelTransform)
                {
                    Destroy(child.gameObject);
                }

                // Instantiate new model.
                Instantiate(modelContainer.DefaultUnpoweredModels[pieceBase.TypeIndex], defaultModelTransform);
            }

            // Custom Model.
            if(customModelTransform.childCount > 0)
            {
                // Destroy current model.
                foreach(Transform child in customModelTransform)
                {
                    Destroy(child.gameObject);
                }

                // Instantiate new model.
                Instantiate(modelContainer.CustomUnpoweredModels[pieceBase.TypeIndex], customModelTransform);
            }
        }

        /// <summary>
        /// Handles the behavior for powering off ending pieces.
        /// If this ending piece was powered on in previous puzzle state, OnPoweredOff event will be invoked.
        /// </summary>
        public void PowerOffEndingPiece()
        {
            // If this is an ending piece.
            if (tag == "EndingCircuitPiece")
            {
                // If this piece was already unpowered during previous puzzle state or it is currently powered, OnPoweredOff event will not be invoked.
                if (wasPreviouslyPowered == false || isPowered)
                {
                    return;
                }

                // During next power check, we will know now this piece was not previously powered.
                wasPreviouslyPowered = false;

                // If the puzzle is in single mode.
                if (puzzleSettings.IsGrouped == false)
                {
                    // Get EndingPieceEvents reference and invoke OnPoweredOff event.
                    EndingPieceEvents ending = GetComponent<EndingPieceEvents>();
                    ending.OnPoweredOff.Invoke();
                }
            }
        }

        /// <summary>
        /// Handles the behavior for powering on ending pieces.
        /// If this ending piece was not powered on in previous puzzle state, OnPoweredOn event will be invoked.
        /// </summary>
        public void PowerOnEndingPiece()
        {
            // If this is an ending piece.
            if (tag == "EndingCircuitPiece")
            {
                // If this piece was already powered during previous puzzle state, OnPoweredOn event will not be invoked.
                if (wasPreviouslyPowered)
                {
                    return;
                }

                // During next power check, we will know now this piece was previously powered.
                wasPreviouslyPowered = true;

                // If puzzle is in single mode.
                if(puzzleSettings.IsGrouped == false)
                {
                    // Get EndingPieceEvents reference and invoke OnPoweredOn event.
                    EndingPieceEvents ending = GetComponent<EndingPieceEvents>();
                    ending.OnPoweredOn.Invoke();

                    // Set completed status as true for one time completion puzzles.
                    puzzleManager.Completed = true;

                    // If this is a one time completion puzzle, for end interaction on puzzle completion.
                    if (puzzleSettings.OneTimeCompletion)
                    {
                        interactPoint.EndInteraction();
                    }
                }
            }
        }
        #endregion

        #region PRIVATE METHODS
        /// <summary>
        /// Get the pieces connected to a wire.
        /// </summary>
        /// <param name="wireIndex"></param>
        private void GetConnectedPiece(int wireIndex)
        {
            foreach(PowerManager piece in wires[wireIndex].Connections)
            {
                connectedPieces.Add(piece);
            }
        }
        #endregion
    }
}