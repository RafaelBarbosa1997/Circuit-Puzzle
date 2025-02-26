using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class PieceBase : MonoBehaviour
{
    #region FIELDS
    // Used to identify what the type of this piece.
    [SerializeField]
    private int typeIndex;

    #endregion

    #region PROPERTIES
    public int TypeIndex { get => typeIndex; set => typeIndex = value; }
    #endregion
}
