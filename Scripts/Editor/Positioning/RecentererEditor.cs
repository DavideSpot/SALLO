using UnityEngine;
using UnityEditor;

namespace SALLO
{
    [CustomEditor(typeof(CameraHandler))]
    public class RecentererEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CameraHandler targetInEditor = (CameraHandler)target;
            if (GUILayout.Button("\nRecenter Observer\n"))
            {
                targetInEditor.Recenter();
            }
        }
    }
}