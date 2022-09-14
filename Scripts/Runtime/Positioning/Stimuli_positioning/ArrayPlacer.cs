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
using System;

namespace SALLO
{
    /// <summary>
    /// <strong>Stimuli Positioning</strong> component. Control the spatial relationships between a set of gameobjects
    /// </summary>
    public class ArrayPlacer : MonoBehaviour
    {
        /// <summary>
        /// The desired azimuth angle between each consecutive pair of GameObjects in the controlled set 
        /// </summary>
        public float AzimuthBetween;

        /// <summary>
        /// Azimuth shift common to all the GameObjects in the controlled set
        /// </summary>
        [SerializeField]
        private float azimuthOffset = 0f;
        public float AzimuthOffset { get => azimuthOffset; set => UpdateOffset(value); }
        /// <summary>
        /// Radius common to all the GameObjects in the controlled set
        /// </summary>
        public float Radius;
        /// <summary>
        /// Height (elevation) common to all the GameObjects in the controlled set
        /// </summary>
        public float Height;

        /// <summary>
        /// The controlled set
        /// </summary>
        [SerializeField]
        private List<CylindricalCoordinates> array;

        /// <summary>
        /// Array of keyword tags
        /// </summary>
        /// <remarks>
        /// If the parameter <see cref="Exclude"></see> is true, the GameObjects with one of the tags in this array will not be included in the controlled set.
        /// </remarks>
        public string[] Keys = null;
        /// <summary>
        /// decide whether to exclude the GameObjects tagged with one of the strings contained in <see cref="Keys"/>.
        /// </summary>
        public bool Exclude = true;


        private void OnEnable()
        {
            if (array == null)
                array = new List<CylindricalCoordinates>();
            SetPositions();
        }

        /// <summary>
        /// Fill the set of GameObjects controlled by this component.
        /// </summary>
        /// <param name="keys">array of keyword tags. See <see cref="Keys"/> </param>
        /// <param name="exclude"> decide whether to exclude the GameObjects whose tag is in <see cref="Exclude"/> </param>
        /// <returns></returns>
        [HideInInspector]
        public List<CylindricalCoordinates> MakeList(string[] keys = null, bool exclude = true) //include or exclude
        {
            array = new List<CylindricalCoordinates>();
            foreach (Transform child in transform)
                if (exclude ^ keys?.Contains(child.tag) ?? false)
                    array.Add(child.GetComponent<CylindricalCoordinates>());
            return array;
        }

        /// <summary>
        /// Update the positions of the controlled set
        /// </summary>
        public void SetPositions()
        {
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            if (!(array.Count > 0)) MakeList(Keys, Exclude);

            for (int i = 0; i < array.Count; i++)
            {
                array[i].ToTransform(Radius, LocalAzimuth(AzimuthBetween, i), Height);
            }
        }

        /// <summary>
        /// Update the common azimuth shift in all the items of the controlled set.
        /// </summary>
        /// <param name="offset">The desired common azimuth shift</param>
        private void UpdateOffset(float offset)
        {
            azimuthOffset = offset;
            for (int i = 0; i < array.Count; i++)
            {
                array[i].Azimuth = LocalAzimuth(AzimuthBetween, i);
            }
        }
        /// <summary>
        /// Computes the azimuth angle for a specific item, given the common azimuth parameters and the item order in the controlled set
        /// </summary>
        /// <param name="azimuth">The desired azimuthal angle between each consecutive pair of items in the controlled set</param>
        /// <param name="i">the index of the target item</param>
        /// <returns>The azimuth angle for the given item</returns>
        private float LocalAzimuth(float azimuth, int i) => azimuthOffset + (i * azimuth) - (azimuth * ((float)array.Count - 1) / 2);

        /// <summary>
        /// Utility method to jitter the common azimuth shift of the controlled set
        /// </summary>
        /// <param name="amplitude">The maximum value for the jitter</param>
        public void Jitter(int amplitude) => AzimuthOffset = UnityEngine.Random.Range(-amplitude, amplitude);
    }
}