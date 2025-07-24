using UnityEngine;

namespace DinoFracture
{
    /// <summary>
    /// Triggers a fracture + explosion when this game object is
    /// collided with.
    /// 
    /// This script does not need to be applied on a fracturing game object.
    /// </summary>
    public class TriggerExplosionOnCollision : ExplodeOnFracture
    {
        /// <summary>
        /// List of explosions to trigger
        /// </summary>
        [UnityEngine.Tooltip("List of explosions to trigger")]
        public FractureGeometry[] Explosives;


        private void OnCollisionEnter(Collision col)
        {
            for (int i = 0; i < Explosives.Length; i++)
            {
                if (Explosives[i] != null && Explosives[i].gameObject.activeSelf)
                {
                    // This ensures OnFracture() will be called on us
                    Explosives[i].Fracture().SetCallbackObject(this);
                }
            }
        }
    }
}