using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    public class AdvancedInteractEvents : MonoBehaviour
    {
        [SerializeField]
        private FirstPersonMovement fpsMove;

        [SerializeField]
        private FirstPersonLook fpsLook;

        public void DisableFPSMovement()
        {
            fpsMove.enabled = false;

            fpsLook.enabled = false;
        }

        public void EnableFPSMovement()
        {
            fpsMove.enabled = true;

            fpsLook.enabled = true;
        }
    }
}