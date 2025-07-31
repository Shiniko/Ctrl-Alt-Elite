using System.Collections.Generic;
using UnityEditor;

namespace DinoFracture.Editor
{
    [CustomEditor(typeof(RuntimeFracturedGeometry))]
    [CanEditMultipleObjects()]
    public class RuntimeFracturedGeometryEditor : FractureGeometryEditor
    {
        private static Dictionary<string, PropertyName> _sRuntimeFractureProperties;

        static RuntimeFracturedGeometryEditor()
        {
            _sRuntimeFractureProperties = new Dictionary<string, PropertyName>();
            AddPropertyName(_sRuntimeFractureProperties, "Asynchronous");
        }

        [MenuItem("CONTEXT/RuntimeFracturedGeometry/Convert To Pre-Fracture")]
        static void ConvertToRuntimeFracture(MenuCommand command)
        {
            RuntimeFracturedGeometry geom = command.context as RuntimeFracturedGeometry;
            if (geom == null)
            {
                return;
            }

            ConvertComponentTo<PreFracturedGeometry>(geom);
        }

        public override void OnInspectorGUI()
        {
            DrawCommonFractureProperties();

            Space(10);

            DrawFractureProperties(_sRuntimeFractureProperties);

            Space(10);

            DrawFractureEventProperties();
        }
    }
}