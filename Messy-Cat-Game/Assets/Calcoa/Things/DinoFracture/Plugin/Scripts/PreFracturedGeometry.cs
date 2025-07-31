using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DinoFracture
{
    /// <summary>
    /// Apply this component to any game object you wish to pre-fracture.
    /// Pre-fracturing is a way of baking fracture pieces into the scene.
    /// Each time the object is fractured, the same set of pieces will
    /// activate.  This is very useful when creating a large number of
    /// pieces or high poly meshes, which would be too slow to create at
    /// runtime.  The pieces will be in the scene as a disabled root object
    /// with piece children.  When the object is fractured, those pieces
    /// will activate.
    /// </summary>
    public class PreFracturedGeometry : FractureGeometry
    {
        /// <summary>
        /// A reference to the root of the pre-fractured pieces.
        /// This is not normally set manually.  Instead, you press
        /// the “Create Fractures” button in the inspector window
        /// to generate the fracture immediately.  
        /// </summary>
        /// <remarks>The “Create Fractures” button is only intended to be used in edit mode; not game mode.</remarks>
        [UnityEngine.Tooltip("A reference to the root of the pre-fractured pieces. This is not normally set manually.  Instead, you press the “Create Fractures” button in the inspector window to generate the fracture immediately.  ")]
        [ReadOnly]
        public GameObject GeneratedPieces;

        /// <summary>
        /// The encapsulating bounds of the entire set of pieces.  In local space.
        /// </summary>
        [UnityEngine.Tooltip("The encapsulating bounds of the entire set of pieces.  In local space.")]
        public Bounds EntireMeshBounds;

        private bool _preFracturing;

        public bool IsPreFracturing => _preFracturing;

        public PreFracturedGeometry()
        {
            SeparateDisjointPieces = true;
        }

        private void Start()
        {
            Prime();
        }

        /// <summary>
        /// Primes the pre-fractured pieces when the game starts by
        /// activating them and then deactivating them.  This avoids
        /// a large delay on fracture if there are a lot of rigid bodies.
        /// </summary>
        public void Prime()
        {
            if (GeneratedPieces != null)
            {
                bool activeSelf = gameObject.activeSelf;
                gameObject.SetActive(false);

                GeneratedPieces.SetActive(true);
                GeneratedPieces.SetActive(false);

                gameObject.SetActive(activeSelf);
            }
        }

        public AsyncFractureResult GenerateFractureMeshes()
        {
            return GenerateFractureMeshes(Vector3.zero);
        }

        public AsyncFractureResult GenerateFractureMeshes(Vector3 localPoint)
        {
            _preFracturing = true;

            return StartFracture(localPoint);
        }

        public void ClearGeneratedPieces(bool deletePieces)
        {
            if (GeneratedPieces != null)
            {
                if (deletePieces)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(GeneratedPieces);
                    }
                    else
                    {
                        DestroyImmediate(GeneratedPieces);
                    }
                }

                GeneratedPieces = null;
            }
        }

        protected override AsyncFractureResult FractureInternal(Vector3 localPos)
        {
            AsyncFractureResult result;

            _preFracturing = false;

            if (gameObject.activeSelf)
            {
                if (GeneratedPieces == null)
                {
                    if (Application.isPlaying)
                    {
                        Logger.Log(LogLevel.UserDisplayedWarning, "DinoFracture: Creating pre-fractured pieces at runtime. This can be slow if there a lot of pieces. Use GenerateFractureMeshes() if this was intended.", gameObject);
                    }

                    result = StartFracture(localPos);
                }
                else
                {
                    result = new AsyncFractureResult();
                    result.SetResult(new OnFractureEventArgs(this, EntireMeshBounds, GeneratedPieces, null));
                }
            }
            else
            {
                result = new AsyncFractureResult();
                result.SetResult(new OnFractureEventArgs(this, new Bounds(), null, null));
            }

            return result;
        }

        protected virtual AsyncFractureResult StartFracture(Vector3 localPoint)
        {
            if (IsProcessingFracture)
            {
                Logger.Log(LogLevel.UserDisplayedError, "DinoFracture: Cannot start a fracture while a fracture is running.", gameObject);
                return null;
            }

            // Don't clear the pieces when fracturing at edit time. The editor
            // script will handle changing out the piece. This allows us to save
            // the old results on failure and also not mark this game object as
            // dirty if the values don't change.
            if (Application.isPlaying)
            {
                ClearGeneratedPieces(deletePieces: true);
            }

            return StartFracture(localPoint, hideAfterFracture: false);
        }

        protected override FractureDetails CreateFractureDetails(Vector3 localPos)
        {
            var ret = base.CreateFractureDetails(localPos);

            if (ret != null)
            {
                ret.Asynchronous = !FractureEngineBase.ForceSynchronousPreFractureInEditor;

                if (VertexMergingPolicy == VertexMergingPolicy.Default)
                {
                    if (Application.isPlaying)
                    {
                        ret.VertexMergingPolicy = VertexMergingPolicy.Simple;
                    }
                    else
                    {
                        ret.VertexMergingPolicy = VertexMergingPolicy.Advanced;
                    }
                }
            }

            return ret;
        }

        private void EnableFracturePieces()
        {
            GeneratedPieces.transform.position = transform.position;
            GeneratedPieces.transform.rotation = transform.rotation;
            GeneratedPieces.SetActive(true);
        }

        protected internal override void OnFracture(OnFractureEventArgs args)
        {
            if (_preFracturing)
            {
                if (TryGetComponent(out ChipOnFracture chipOnFractureComp))
                {
                    GameObject unchippedRoot = chipOnFractureComp.OnPrefractureComplete(args);
                    if (unchippedRoot != null)
                    {
                        if (args.FracturePiecesRootObject != null)
                        {
                            GameObject newRoot = new GameObject(name + " - Fracture and Unchipped Root");
                            newRoot.transform.localPosition = args.FracturePiecesRootObject.transform.localPosition;
                            newRoot.transform.localRotation = args.FracturePiecesRootObject.transform.localRotation;
                            newRoot.transform.localScale = args.FracturePiecesRootObject.transform.localScale;
                            newRoot.transform.SetParent(args.FracturePiecesRootObject.transform.parent, worldPositionStays: false);

                            args.FracturePiecesRootObject.transform.SetParent(newRoot.transform, worldPositionStays: false);
                            unchippedRoot.transform.SetParent(newRoot.transform, worldPositionStays: false);

                            args.FracturePiecesRootObject = newRoot;
                        }
                        else
                        {
                            args.FracturePiecesRootObject = unchippedRoot;
                        }
                    }
                }

                if (Application.isPlaying)
                {
                    if (PopulatePreFractureFields(args))
                    {
                        Prime();
                    }
                }
                else
                {
                    // The editor script will take care of assignment
                    // of the new mesh
                }
            }
            else
            {
                if (Application.isPlaying)
                {
                    if (PopulatePreFractureFields(args))
                    {
                        EnableFracturePieces();
                    }

                    // Always disable because the game might require
                    // the mesh be gone and we don't want to interfere with that.
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.Assert(false, "Do not call Fracture() in edit mode. Call GenerateFractureMeshes() to generate fracture pieces.");
                }
            }

            base.OnFracture(args);
        }

        /// <summary>
        /// Populates the pre-fracture generated pieces and bounds fields.
        /// </summary>
        /// <remarks>
        /// Internal fields will be unchanged if the incoming data is invalid.
        /// </remarks>
        /// <returns>True if the incoming data is valid, false if it is not.</returns>
        protected bool PopulatePreFractureFields(OnFractureEventArgs args)
        {
            if (args.IsValid)
            {
                GeneratedPieces = args.FracturePiecesRootObject;
                EntireMeshBounds = args.OriginalMeshBounds;

                return true;
            }

            return false;
        }
    }
}