using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    public class AdvancedInteract : MonoBehaviour
    {
        #region FIELDS
        // Reference to camera transform to send out raycast.
        [SerializeField]
        private Transform cameraTransform;

        // Reference to interact point we will get from raycast.
        private InteractPoint interactPoint;
        #endregion

        #region UNITY METHODS
        private void Update()
        {
            // Begin and end interaction.
            BeginInteraction();

            EndInteraction();

            // Puzzle controls, only runs if we have the interactPoint reference.
            if(interactPoint != null)
            {
                MoveUp();

                MoveDown();

                MoveLeft();

                MoveRight();

                RotatePieceLeft();

                RotatePieceRight();
            }
        }
        #endregion

        #region PRIVATE METHODS
        /// <summary>
        /// Sends out a raycast and tries to get the InteractPoint component.
        /// If successful, begins puzzle interaction.
        /// </summary>
        private void BeginInteraction()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                LayerMask mask = LayerMask.GetMask("CircuitInteraction");

                if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit info, 100f, mask))
                {
                    interactPoint = info.transform.GetComponent<InteractPoint>();

                    interactPoint.BeginInteraction();
                }
            }
        }

        private void MoveUp()
        {
            if(Input.GetKeyDown(KeyCode.W)) 
            {
                interactPoint.MoveSelectionVertical(1);
            }
        }

        private void MoveDown()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                interactPoint.MoveSelectionVertical(-1);
            }
        }

        private void MoveLeft()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                interactPoint.MoveSelectionHorizontal(-1);
            }
        }

        private void MoveRight()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                interactPoint.MoveSelectionHorizontal(1);
            }
        }

        private void RotatePieceLeft()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                interactPoint.RotatePieceLeft();
            }
        }

        private void RotatePieceRight()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                interactPoint.RotatePieceRight();
            }
        }

        private void EndInteraction()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                interactPoint.EndInteraction();
            }
        }
        #endregion
    }
}