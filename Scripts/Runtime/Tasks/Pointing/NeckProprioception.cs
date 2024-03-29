﻿/*  Copyright (C) 2022 Unit for Visually Impaired People (UVIP) - Fondazione Istituto Italiano di Tecnologia (IIT)
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
    /// <strong>Task</strong> component. Implements the neck proprioception assessment task
    /// </summary>
    public class NeckProprioception : Task
    {
        PositionWatcher StartingPoint;

        /// <summary>
        /// The UI test where to show the target and/or the current angle
        /// </summary>
        private TextMesh Target;
        /// <summary>
        /// The material of the pointer.
        /// </summary>
        /// <remarks> Change it programmatically to give visual feedback to the experimentare about the trial advancement. </remarks>
        public Material Pointer;

        private void Awake()
        {
            sense = SensoryChannel.PROPRIOCEPTIVE;
            GetComponent<PitchController>().enabled = false;
        }

        private void OnEnable()
        {
            Target = GetComponentInChildren<TextMesh>();

            StartingPoint = GetComponent<PositionWatcher>();

            InputWatcher.OnMoreTriggersPressed += GetAnswer;
        }

        /// <inheritdoc/>
        protected override void GetAnswer<T>(T answer)
        {
            if (canAnswer)
            {
                float theta = StartingPoint.observer.eulerAngles.y;
                Answer = (theta % 360 > 180 ? theta - 360 : theta).ToString("F1");
                onParticipantAnswered?.Invoke();
                canAnswer = false;
            }
        }

        private void OnDisable()
        {
            InputWatcher.OnMoreTriggersPressed -= GetAnswer;
        }

        /// <inheritdoc/>
        public override void Run(float? timeOn = null, float? timeOff = null)
        {
            canAnswer = false;
            if (CheckPosition)
                FindReference();
            else
                FindStimulus();

        }

        /// <summary>
        /// Start the trial phase of the starting angle search.
        /// </summary>
        void FindReference()
        {
            //Target.
            StartingPoint.CheckAngle(transform.parent.eulerAngles.y, 60, tolerance: 2.5f);
            Pointer.SetColor("_EmissionColor", Color.cyan);
            Pointer.EnableKeyword("_EMISSION");
            StartingPoint.PositionFound = FindStimulus;

            ;
        }
        /// <summary>
        /// Start the trial phase of the stimulus angle search.
        /// </summary>
        void FindStimulus()
        {
            Target.transform.Rotate(Vector3.right, 90, Space.Self);
            Pointer.SetColor("_EmissionColor", Color.clear);
            Pointer.EnableKeyword("_EMISSION");
            StartingPoint.CheckAngle(Stimulus.transform.eulerAngles.y, 60, tolerance: 2.5f);
            StartingPoint.PositionFound = () => { canAnswer = true; };
            StartCoroutine(WriteError());
        }

        /// <summary>
        /// Output to the UI the angular distance between current and target observer angle
        /// </summary>
        IEnumerator WriteError()
        {
            RunStarted?.Invoke();
            while (!canAnswer)
            {
                yield return null;
                int error = (int)StartingPoint.observer_stimuli_angle % 360;
                Target.text = Angle.WrapTo180(error).ToString();
            }
            Target.text = string.Empty;
            Pointer.SetColor("_EmissionColor", Color.red);
            Pointer.EnableKeyword("_EMISSION");
            RunFinished?.Invoke();
        }

        protected override IEnumerator ShowStimulus(CylindricalCoordinates stimulus, float timeOn)
        {
            throw new System.NotImplementedException();
        }

        protected override void PositionCheckSetUp()
        {
            throw new System.NotImplementedException();
        }
    }
}