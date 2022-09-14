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

using System.Collections;
using UnityEngine;

namespace SALLO
{
    /// <summary>
    /// <strong>Stimuli Positioning</strong> component. Keep the height of a target GameObject at the same height as the GameObject with this component.
    /// </summary>
    public class HeightController : MonoBehaviour
    {
        /// <summary>
        /// The target GameObject
        /// </summary>
        public Transform taskPlaceHolder;
        // Start is called before the first frame update
        private void Start()
        {
            StartCoroutine(EvenHeight());
        }
        /// <summary>
        /// Update the target GameObject's height
        /// </summary>
        /// <returns></returns>
        IEnumerator EvenHeight()
        {
            yield return new WaitForEndOfFrame();
            taskPlaceHolder.position = new Vector3(taskPlaceHolder.position.x, transform.position.y, taskPlaceHolder.position.z);
        }
    }
}