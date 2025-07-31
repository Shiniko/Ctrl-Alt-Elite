using UnityEngine;

namespace DinoFracture
{
    /// <summary>
    /// This will pre-fracture geometry at runtime when this
    /// script starts. Such behavior is useful when you want
    /// the benefits of pre-fracturing (no runtime cost when
    /// the object is actually fractured) without the extra
    /// package overhead.
    /// </summary>
    /// <remarks>
    /// It is best to use this script during a level loading
    /// screen if pre-fracturing with a lot of pieces.
    /// </remarks>
    public class PreFractureOnStart : MonoBehaviour
    {
        /// <summary>
        /// The fracture geometry to pre-fracture.
        /// </summary>
        [Tooltip("The fracture geometry to pre-fracture.")]
        public PreFracturedGeometry PreFracturedGeometry;

        /// <summary>
        /// Optional local point to fracture. Only used
        /// if the Fracture Size on the fracture geometry
        /// is not zero.
        /// </summary>
        [Tooltip("Optional local point to fracture. Only used if the Fracture Size on the fracture geometry is not zero.")]
        public Vector3 LocalFracturePoint;

        public void Reset()
        {
            TryGetComponent(out PreFracturedGeometry);
        }

        public void Start()
        {
            if (PreFracturedGeometry != null)
            {
                var asyncOp = PreFracturedGeometry.GenerateFractureMeshes(LocalFracturePoint);

                // This will be cleaned up after firing automatically
                asyncOp.OnFractureComplete += OnFractureComplete;
            }
        }

        private void OnFractureComplete(OnFractureEventArgs args)
        {
            if (args.OriginalObject == PreFracturedGeometry)
            {
                // If copying this script, you can run code here after fracture completes

                //Debug.Log($"Fracturing complete. [Game Object: {PreFracturedGeometry.name}] [Success: {args.IsValid}]");
            }
        }
    }
}