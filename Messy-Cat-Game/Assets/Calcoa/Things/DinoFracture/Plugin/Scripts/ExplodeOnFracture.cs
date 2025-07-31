using UnityEngine;

namespace DinoFracture
{
    /// <summary>
    /// When applied to a game object with a FractureGeometry component this
    /// will apply an explosive point to the point of fracture. If there is
    /// no point of fracture, the center of the object will be used.
    /// </summary>
    public class ExplodeOnFracture : MonoBehaviour
    {
        /// <summary>
        /// The force behind the explosions
        /// </summary>
        [UnityEngine.Tooltip("The force behind the explosions")]
        public float Force;

        /// <summary>
        /// The radius of the explosions
        /// </summary>
        [UnityEngine.Tooltip("The radius of the explosions")]
        public float Radius;

        /// <summary>
        /// Automatically called by FractureEngine when fracturing is complete
        /// </summary>
        /// <param name="args"></param>
        protected void OnFracture(OnFractureEventArgs args)
        {
            if (args.IsValid)
            {
                var scale = args.OriginalObject.transform.localScale;
                var bounds = args.OriginalMeshBounds;
                Vector3 localCenter = new Vector3(bounds.center.x * scale.x, bounds.center.y * scale.y, bounds.center.z * scale.z);

                if (args.FractureDetails is ShatterDetails shatterDetails)
                {
                    if (shatterDetails.FractureSize.Value > 0)
                    {
                        localCenter = shatterDetails.FractureCenter;
                    }
                }

                Explode(args.FracturePiecesRootObject, localCenter);
            }
        }

        private void Explode(GameObject root, Vector3 localCenter)
        {
            Vector3 center = root.transform.localToWorldMatrix.MultiplyPoint(localCenter);
            Transform rootTrans = root.transform;
            for (int i = 0; i < rootTrans.childCount; i++)
            {
                Transform pieceTrans = rootTrans.GetChild(i);
                Rigidbody body = pieceTrans.GetComponent<Rigidbody>();
                if (body != null)
                {
                    Vector3 forceVector = (pieceTrans.position - center);
                    float dist = forceVector.magnitude;

                    // Normalize the vector and scale it by the explosion radius
                    forceVector *= Mathf.Max(0.0f, Radius - dist) / (Radius * dist);

                    // Scale by the force amount
                    forceVector *= Force;

                    body.AddForceAtPosition(forceVector, center);
                }
            }
        }
    }
}