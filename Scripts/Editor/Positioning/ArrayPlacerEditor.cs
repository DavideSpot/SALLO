using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace SALLO
{ 
    [CustomEditor(typeof(ArrayPlacer))]
    public class ArrayPlacerEditor : Editor
    {
        /// <summary>
        /// Add two buttons to the ArrayPlacer default inspector GUI:
        /// <list type="bullet">
        /// <item><description><c>Get Array</c> calls the <c>MakeList</c> method,
        /// to import the available items in the array under control of the <c>ArrayPlacer</c> instance.
        /// </description></item>
        /// <item><description><c>Update Positions</c> calls the <c>SetPositions</c> method,
        /// to update the position of the items according to the current parameters set.
        /// </description></item>
        /// </list>
        /// <see cref="ArrayPlacer"/>
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ArrayPlacer targetInEditor = (ArrayPlacer)target;
            if (GUILayout.Button("Get Array"))
            {
                targetInEditor.MakeList(targetInEditor.Keys, targetInEditor.Exclude);
            }
            if (GUILayout.Button("Update Positions"))
            {
                targetInEditor.SetPositions();
            }
        }
    }
}