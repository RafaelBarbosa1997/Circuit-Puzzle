using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BoardStateSerializer : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private List<PieceInfo> boardPiecesInfo;

    private void Awake()
    {
        if(boardPiecesInfo == null)
        {
            boardPiecesInfo = new List<PieceInfo>();
        }
    }

    public void AddPiece(GameObject piecePrefabToAdd, int matrixRowToAdd, int matrixColumnToAdd)
    {
        PieceInfo pieceInfo = new PieceInfo()
        {
            piecePrefab = piecePrefabToAdd,
            matrixRow = matrixRowToAdd,
            matrixColumn = matrixColumnToAdd
        };

        boardPiecesInfo.Add(pieceInfo);
    }

    public List<PieceInfo> GetSavedBoardState()
    {
        if(boardPiecesInfo == null ||  boardPiecesInfo.Count == 0)
        {
            return null;
        }

        else
        {
            return boardPiecesInfo;
        }
    }

    public void ResetBoardState()
    {
        boardPiecesInfo.Clear();
    }
}
