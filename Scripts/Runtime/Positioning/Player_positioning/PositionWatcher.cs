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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SALLO
{
    /// <summary>
    /// <strong>Player Positioning</strong> component. Useful to track the position (orientation) of an observer with respect to any target angle
    /// and respond accordingly
    /// </summary>
    [RequireComponent(typeof(PitchController))]
    [RequireComponent(typeof(Task))]
    public class PositionWatcher : MonoBehaviour
    {
        private Task task;
        private PitchController pc;
        [HideInInspector]
        public Transform observer;
        public bool inPosition { get; private set; }
        private float TargetAngle = 0f;
        public float observer_stimuli_angle
        {
            get => Mathf.Abs(TargetAngle - Angle.WrapTo180(observer.eulerAngles.y));
        }
        /// <summary>
        /// called when the observer finds the target angle
        /// </summary>
        public UnityAction PositionFound;
        /// <summary>
        /// flag to indicate whether the observer holds the target orientation while performing some actions
        /// </summary>
        public bool PositionKept;

        /// <summary>
        /// check whether the observer holds the target orientation during the stimuli presentation
        /// </summary>
        void OnEnable()
        {
            observer = GameObject.Find("Main Camera").transform;
            pc = GetComponent<PitchController>();
            pc.reference = observer;

            task = GetComponent<Task>();
            if (task.CheckPosition)
            {
                task.RunStarted += delegate () { StartCoroutine(IsKeepingPosition()); };
                task.RunFinished += delegate () { if (!PositionKept) StopCoroutine(IsKeepingPosition()); };
            }
        }
        /// <summary>
        /// Public method to check if the observer finds a target angle (orientation)
        /// </summary>
        /// <remarks>the orientation check is performed in the private method <see cref="CountToFrame"/> </remarks>
        /// <param name="targetAngle">the target angle (orientation)</param>
        /// <param name="frameLimit">the amount of frames that the observer should hold their orientation to consider the target angle (orientation) as found </param>
        /// <param name="tolerance">the tolerance in the target angle accuracy ([angle-tolerance,angle+tolerance]) </param>
        public void CheckAngle(float targetAngle, int? frameLimit = null, float tolerance = 3f)
        {
            inPosition = false;
            TargetAngle = Angle.WrapTo180(targetAngle);
            
            if (task.sense == SensoryChannel.ACOUSTIC || task.sense == SensoryChannel.AUDIOVISUAL) pc.enabled = true;
            StartCoroutine(CountToFrame(frameLimit, tolerance));
        }
        /// <summary>
        /// Private method to check if the observer finds a target angle (orientation)
        /// </summary>
        /// <remarks>invoked in the public method <see cref="CheckAngle"/></remarks>
        /// <param name="frameLimit">the amount of frames that the observer should hold their orientation to consider the target angle (orientation) as found </param>
        /// <param name="tolerance">the tolerance in the target angle accuracy ([angle-tolerance,angle+tolerance]) </param>
        private IEnumerator CountToFrame(int? frameLimit = null, float tolerance = 3f)
        {
            Debug.Log("Searching position...");
            if (frameLimit == null) frameLimit = UnityEngine.Random.Range(45, 75);
            int counter = 0;
            while (counter <= frameLimit)
            {
                counter = observer_stimuli_angle <= tolerance ? counter + 1 : 0;
                yield return null;
            }
            inPosition = true;
            pc.enabled = false;

            yield return null;
            Debug.Log("Position found!");
            PositionFound?.Invoke();
        }
        /// <summary>
        /// public method to check if the observer is mantaining their orientation
        /// </summary>
        /// <param name="tolerance">the tolerance in the target angle accuracy ([angle-tolerance,angle+tolerance]) </param>
        public IEnumerator IsKeepingPosition(float tolerance = 3f)
        {
            PositionKept = true;
            while (PositionKept)
            {
                if (observer_stimuli_angle >= tolerance) PositionKept = false;
                yield return null;
            }
        }



    }
}