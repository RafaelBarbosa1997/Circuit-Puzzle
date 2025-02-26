using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    /// <summary>
    /// This Monobehaviour is placed on a GameObject to prevent it from being deleted from the hierarchy.
    /// Used on children of puzzle, as deleting these GameObjects will cause errors.
    /// </summary>
    [ExecuteInEditMode]
    public class DeleteIntercept : MonoBehaviour
    {
        
    }
}