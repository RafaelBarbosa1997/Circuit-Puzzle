using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CircuitPuzzle
{
    public class GroupedEndingEvents : MonoBehaviour
    {
        #region FIELDS
        // Unity event that runs when this piece is powered.
        [SerializeField]
        private UnityEvent onPoweredOn;

        // Unity event that runs when this piece is powered off.
        [SerializeField]
        private UnityEvent onPoweredOff;
        #endregion

        #region PROPERTIES
        public UnityEvent OnPoweredOn { get => onPoweredOn; private set => onPoweredOn = value; }
        public UnityEvent OnPoweredOff { get => onPoweredOff; private set => onPoweredOff = value; }
        #endregion
    }
}
