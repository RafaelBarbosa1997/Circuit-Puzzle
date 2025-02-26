using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CircuitPuzzle
{
    public class InteractPoint : MonoBehaviour
    {
        #region FIELDS
        // Reference to the puzzle manager component in parent transform.
        private PuzzleManager puzzleManager;

        // Reference to puzzle settings, used to check if puzzle is one time or continuous.
        private PuzzleSettings puzzleSettings;
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            // Get puzzle manager reference.
            puzzleManager = transform.parent.GetComponent<PuzzleManager>();

            // Get puzzle settings reference.
            puzzleSettings = transform.parent.GetComponent<PuzzleSettings>();
        }
        #endregion

        #region EVENTS
        // Events that will be called when the player starts and stops interacting with the interact point.
        [SerializeField]
        private UnityEvent OnBeginInteraction;
        [SerializeField]
        private UnityEvent OnEndInteraction;
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Calls the puzzle manager's StartPuzzle method, followed by the OnBeginInteraction event.
        /// Allows user to customize what happens when puzzle interaction begins.
        /// </summary>
        public void BeginInteraction()
        {
            // If the puzzle is one time completion and it has already been completed, we cannot interact with it again.
            if((PuzzleManager.ActiveInstance == null) && (puzzleSettings.OneTimeCompletion == false || puzzleManager.Completed == false))
            {
                // Start puzzle.
                puzzleManager.StartPuzzle();

                // Invoke event.
                OnBeginInteraction.Invoke();
            }
        }

        /// <summary>
        /// Just calls the same function from the puzzle manager.
        /// This way user doesn't need to check if active instance is null manually.
        /// Use -1 to move down and 1 to move up.
        /// </summary>
        /// <param name="direction"></param>
        public void MoveSelectionVertical(int direction)
        {
            if(PuzzleManager.ActiveInstance != null)
            {
                PuzzleManager.ActiveInstance.MoveSelectionVertical(direction);
            }
        }

        /// <summary>
        /// Just calls the same function from the puzzle manager.
        /// This way user doesn't need to check if active instance is null manually.
        /// Use -1 to move left and 1 to move right.
        /// </summary>
        /// <param name="direction"></param>
        public void MoveSelectionHorizontal(int direction)
        {
            if(PuzzleManager.ActiveInstance != null)
            {
                PuzzleManager.ActiveInstance.MoveSelectionHorizontal(direction);
            }
        }

        /// <summary>
        /// Just calls the same function from the puzzle manager.
        /// This way user doesn't need to check if active instance is null manually.
        /// </summary>
        public void RotatePieceLeft()
        {
            if(PuzzleManager.ActiveInstance!= null)
            {
                PuzzleManager.ActiveInstance.RotatePieceLeft();
            }
        }

        /// <summary>
        /// Just calls the same function from the puzzle manager.
        /// This way user doesn't need to check if active instance is null manually.
        /// </summary>
        public void RotatePieceRight()
        {
            if(PuzzleManager.ActiveInstance!= null)
            {
                PuzzleManager.ActiveInstance.RotatePieceRight();
            }
        }

        /// <summary>
        /// Calls the puzzle manager's EndPuzzle method, followed by OnEndInteraction event.
        /// Allows user to customize what happens when puzzle interaction ends.
        /// </summary>
        public void EndInteraction()
        {
            if(PuzzleManager.ActiveInstance != null)
            {
                // End puzzle.
                PuzzleManager.ActiveInstance.EndPuzzle();

                //Invoke event.
                OnEndInteraction.Invoke();
            }
        }
        #endregion
    }
}