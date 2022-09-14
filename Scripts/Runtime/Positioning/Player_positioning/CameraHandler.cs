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

using UnityEngine;

namespace SALLO
{
    /// <summary>
    /// Handle the VR camera shifts
    /// </summary>
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField]
        Vector3 PositionCheck, RotationCheck;
        /// <summary>
        /// Counterbalance the VR <see cref="Camera"/> position shifts to keep it at the virtual space origin
        /// </summary>
        /// <remarks> Give the "Camera" GameObject a parent and update the parent position in the opposite direction of the camera position </remarks>
        void Update()
        {
            transform.parent.position = -transform.localPosition;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5);
        }
        /// <summary>
        /// display the global position and rotation
        /// </summary>
        private void LateUpdate()
        {
            PositionCheck = transform.position;
            RotationCheck = transform.eulerAngles;
        }
        /// <summary>
        /// Move the camera back to the virtual space origin
        /// </summary>
        public void Recenter()
        {
            transform.parent.SetPositionAndRotation(-transform.localPosition, Quaternion.Inverse(transform.localRotation));
        }
    }
}