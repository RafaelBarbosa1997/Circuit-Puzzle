using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    public class CameraSwitcher : MonoBehaviour
    {
        private Camera puzzleCamera;

        private Camera previousCamera;

        private void Start()
        {
            puzzleCamera = GetComponent<Camera>();
        }

        public void SwitchToPuzzleCamera()
        {
            previousCamera = Camera.main;

            puzzleCamera.enabled = true;

            previousCamera.enabled = false;
        }

        public void SwitchToPreviousCamera()
        {
            previousCamera.enabled = true;

            puzzleCamera.enabled = false;
        }
    }
}