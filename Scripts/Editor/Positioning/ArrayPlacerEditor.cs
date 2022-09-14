/*  Copyright (C) 2022 Unit for Visually Impaired People (UVIP) - Fondazione Istituto Italiano di Tecnologia (IIT)
    Author: Davide Esposito
    email: davide.esposito@iit.it | spsdvd48@gmail.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

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