using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SALLO
{
    [CustomEditor(typeof(CylindricalCoordinates))]
    public class CylindricalCoordinatesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CylindricalCoordinates targetInEditor = (CylindricalCoordinates)target;
            if (GUILayout.Button("Update Position"))
            {
                targetInEditor.ToTransform();
            }
        }
    }
}