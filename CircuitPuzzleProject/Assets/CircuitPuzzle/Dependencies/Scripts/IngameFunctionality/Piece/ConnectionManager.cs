using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    public class ConnectionManager : MonoBehaviour
    {
        #region FIELDS
        // List of PowerManager components from pieces currently connected to this one.
        private List<PowerManager> connections;

        // Reference to PuzzleManager component in this pieces puzzle board.
        private PuzzleManager puzzleManager;
        #endregion

        #region PROPERTIES
        public List<PowerManager> Connections { get => connections; private set => connections = value; }
        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            // Initialize connections list.
            connections = new List<PowerManager>();

            // Get reference to GameObject containing PuzzleManager component.
            Transform colliderParent = transform.parent;
            Transform pieceParent = colliderParent.parent;
            Transform boardParent = pieceParent.parent;
            Transform puzzleParent = boardParent.parent;

            // Get PuzzleManager component reference.
            puzzleManager = puzzleParent.gameObject.GetComponent<PuzzleManager>();
        }
        #endregion

        #region COLLISIONS
        /// <summary>
        ///  When a gameObject tagged "CircuitWire" enters collider, add its PowerManager component to connections list.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            // When this wire enters collision with another one get its PowerManager component and adds it to connections list.
            if(other.tag == "CircuitWire")
            {
                // Get reference to PowerManager component.
                PowerManager manager = GetPowerManager(other);

                // If PowerManager component is not already in connections list, add it.
                if (!connections.Contains(manager))
                {
                    connections.Add(manager);
                }

                // Check power status whenever a wire get connected.
                if (PuzzleManager.ActiveInstance != null)
                {
                    puzzleManager.CheckPowerStatus();
                }
            }
        }

        /// <summary>
        /// When a gameObject tagged "CircuitWire" enters collider, remove its PowerManager component from connections list.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            // When this wire exits collision with another one the other wire's associated PowerManager component is removed from connections list.
            if(other.tag == "CircuitWire")
            {
                // Get reference to PowerManager component.
                PowerManager manager = GetPowerManager(other);

                // If PowerManager component is in connections list, remove it.
                if (connections.Contains(manager))
                {
                    connections.Remove(manager);
                }

                // Check power status whenever a wire is disconnected.
                if(PuzzleManager.ActiveInstance != null)
                {
                    puzzleManager.CheckPowerStatus();
                }
            }
        }
        #endregion

        #region PRIVATE METHODS
        /// <summary>
        /// Retrive the PowerManager component from a gameObject interacting with a collider.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private PowerManager GetPowerManager(Collider other)
        {
            // Get reference to gameObject containing the PowerManager component.
            GameObject wire = other.gameObject;
            GameObject wireParent = wire.transform.parent.gameObject;
            GameObject piece = wireParent.transform.parent.gameObject;

            // Get reference to PowerManager component.
            PowerManager manager = piece.GetComponent<PowerManager>();

            // Return PowerManager component.
            return manager;
        }
        #endregion
    }
}