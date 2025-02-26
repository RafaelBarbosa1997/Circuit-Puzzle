using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    public class BasicInteract : MonoBehaviour
    {
        #region FIELDS
        // Reference to the interaction point.
        [SerializeField]
        private InteractPoint interactPoint;
        #endregion

        #region UNITY METHODS
        private void Update()
        {
            // Begin interaction.
            if (Input.GetKeyDown(KeyCode.F))
            {
                interactPoint.BeginInteraction();
            }

            // Rotate left.
            if (Input.GetKeyDown(KeyCode.Q))
            {
                interactPoint.RotatePieceLeft();
            }

            // Rotate right.
            if (Input.GetKeyDown(KeyCode.E))
            {
                interactPoint.RotatePieceRight();
            }

            // Move up.
            if (Input.GetKeyDown(KeyCode.W))
            {
                interactPoint.MoveSelectionVertical(1);
            }

            // Move down.
            if (Input.GetKeyDown(KeyCode.S))
            {
                interactPoint.MoveSelectionVertical(-1);
            }

            // Move left.
            if (Input.GetKeyDown(KeyCode.A))
            {
                interactPoint.MoveSelectionHorizontal(-1);
            }

            // Move right.
            if (Input.GetKeyDown(KeyCode.D))
            {
                interactPoint.MoveSelectionHorizontal(1);
            }

            // End interaction.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                interactPoint.EndInteraction();
            }
        }
        #endregion
    }
}