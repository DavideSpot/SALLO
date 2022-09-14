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
using UnityEngine.Events;

namespace SALLO
{
    /// <summary>
    /// The available response patterns.
    /// </summary>
    /// <remarks>
    /// Edit to create the desired response pattern
    /// </remarks>
    public enum triggers
    {
        none,
        right,
        left,
        both
    }
    /// <summary>
    /// <strong>Task</strong> component. Implements the user input - SALLO interface
    /// </summary>
    public class InputWatcher : MonoBehaviour
    {
        /// <summary>
        /// name of the input to be used as right trigger.
        /// </summary>
        public KeyCode rightTrigger;
        /// <summary>
        /// name of the input to be used as left trigger.
        /// </summary>
        public KeyCode leftTrigger;

        /// <summary>
        /// Event for the user press of one response trigger
        /// </summary>
        public static UnityAction<triggers> OnMoreTriggersPressed;
        /// <summary>
        /// Event for the user press of more response triggers together
        /// </summary>
        public static UnityAction<triggers> OnOneTriggerPressed;

        /// <summary>
        /// Check if the user is pressing one response trigger among the available ones
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(leftTrigger) && Input.GetKeyDown(rightTrigger))
                OnMoreTriggersPressed?.Invoke(triggers.right);
            if (Input.GetKeyDown(leftTrigger) && !Input.GetKeyDown(rightTrigger))
                OnOneTriggerPressed?.Invoke(triggers.left);
            if (!Input.GetKeyDown(leftTrigger) && Input.GetKeyDown(rightTrigger))
                OnOneTriggerPressed?.Invoke(triggers.right);
        }
    }
}