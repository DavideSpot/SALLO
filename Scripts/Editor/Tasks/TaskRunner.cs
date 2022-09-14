using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SALLO
{
    [CustomEditor(typeof(Task), true)]
    public class TaskRunner : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Task task = (Task)target;

            if (GUILayout.Button("\nRUN\n"))
            {
                task.Run(task.TimeOn, task.TimeOff);
            }
        }
    }
}