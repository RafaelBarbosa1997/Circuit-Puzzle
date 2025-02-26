using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CircuitPuzzle
{
    [ExecuteInEditMode]
    public class PrefabUnpacker : MonoBehaviour
    {
        //#region FIELDS
        //[SerializeField]
        //private PuzzleCreator creator;
        //#endregion

        //#region UNITY METHODS
        //private void Awake()
        //{
        //    creator = GetComponent<PuzzleCreator>();

        //    // Unpacks the prefab once it is placed on the hierarchy.
        //    // Only does this if it hasn't already been unpacked.
        //    if (PrefabUtility.IsPartOfAnyPrefab(gameObject))
        //    {
        //        PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

        //        creator.UndoCleared = false;
        //    }
        //}
        //#endregion
    }
}