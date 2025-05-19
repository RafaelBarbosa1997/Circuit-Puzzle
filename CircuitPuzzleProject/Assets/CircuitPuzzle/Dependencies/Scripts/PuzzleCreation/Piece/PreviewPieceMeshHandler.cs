using UnityEngine;

namespace CircuitPuzzle
{
    /// <summary>
    /// This class manipulates the preview pieces' MeshRenderer during puzzle creation.
    /// It sets materials and disables or enables meshes, used to reflect pending changes to the puzzle. 
    /// </summary>
    [ExecuteInEditMode]
    public class PreviewPieceMeshHandler : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer baseRenderer;

        [SerializeField]
        private PieceAssetsSO pieceAssets;

        private void EnableMeshes()
        {
            baseRenderer.enabled = true;
        }

        public void SetGreenPreviewMaterial()
        {
            if(baseRenderer.enabled == false)
            {
                EnableMeshes();
            }

            baseRenderer.sharedMaterial = pieceAssets.GreenPreviewMat;
        }

        public void SetRedPreviewMaterial()
        {
            if(baseRenderer.enabled == false)
            {
                EnableMeshes();
            }

            baseRenderer.sharedMaterial = pieceAssets.RedPreviewMat;
        }

        public void DisableMeshes()
        {
            baseRenderer.enabled = false;
        }
    }
}
