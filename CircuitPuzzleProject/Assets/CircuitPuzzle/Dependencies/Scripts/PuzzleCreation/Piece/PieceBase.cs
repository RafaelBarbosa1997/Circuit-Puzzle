using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class PieceBase : MonoBehaviour
{
    #region FIELDS
    // Used to identify what the type of this piece.
    [SerializeField]
    private int typeIndex;

    [SerializeField]
    private MeshRenderer baseRenderer;
    #endregion

    #region PROPERTIES
    public int TypeIndex { get => typeIndex; set => typeIndex = value; }
    #endregion

    public float GetPieceSize()
    {
        float pieceSize = baseRenderer.bounds.size.x;

        return pieceSize;
    }
}
