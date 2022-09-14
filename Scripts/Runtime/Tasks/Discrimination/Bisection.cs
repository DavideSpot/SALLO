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
    /// <strong> Task </strong> component. Implements the space bisection task
    /// <seealso href="https://doi.org/10.1093/brain/awt311"/>
    /// </summary>
    public class Bisection : N_AFC
    {
        /// <summary>
        /// The reference stimuli
        /// </summary>
        public CylindricalCoordinates[] references;

        public GameObject Pointer;
        
        /// <summary>
        /// Decide the side from which sequence starts
        /// </summary>
        public bool? FirstLeft = null;

        private PositionWatcher StartingPoint;


        void OnEnable()
        {
 /* Use for debugging:
            //UpdateDistance();
            //movingPositionsList = new float[] { -20, -10, 0, 10, 20 };
 */
            references[0].gameObject.SetActive(false);
            references[1].gameObject.SetActive(false);
            Stimulus.gameObject.SetActive(false);
            Pointer.SetActive(false);
            Transform BG = transform.GetChild(3);
            BG.localPosition = Vector3.forward * (BG.lossyScale.x / 2f) / Mathf.Tan(Mathf.Deg2Rad * 55.5f);

            StartingPoint = GetComponent<PositionWatcher>();

            RequestPositionCheck(CheckPosition);
            
            
            InputWatcher.OnOneTriggerPressed += GetAnswer;
        }

        private void OnDisable()
        {
            InputWatcher.OnOneTriggerPressed -= GetAnswer;
        }

        /// <summary>
        /// Set the action to take when the target position is found
        /// </summary>
        protected override void PositionCheckSetUp()
        {
            StartingPoint.PositionFound = () =>
            {
                transform.SetParent(StartingPoint.observer);
                Pointer.SetActive(false);
                StartCoroutine(StartSequenceAfter(new WaitForSecondsRealtime(TimeITI)));
            };
        }

        /// <inheritdoc/>
        public override void Run(float? timeOn = null, float? timeOff = null)
        {
            canAnswer = false;

            if (CheckPosition)
            {
                Pointer.SetActive(true);
                StartingPoint.CheckAngle(transform.eulerAngles.y, 60, 2.5f);
            }
            else
            {
                StartCoroutine(StartSequenceAfter(new WaitForSecondsRealtime(TimeITI), timeOn ?? TimeOn, timeOff ?? TimeOff));
            }
        }

        IEnumerator StartSequenceAfter(CustomYieldInstruction whatToWait, float? timeOn = null, float? timeOff = null)
        {
            
            yield return whatToWait;
            StartCoroutine(Sequence(FirstLeft ?? (Random.value > .5f), timeOn ?? TimeOn, timeOff ?? TimeOff));
        }

        /// <summary>
        /// Implements the stimuli sequence for this task
        /// </summary>
        /// <param name="firstLeft"> Decide the side from which sequence starts </param>
        /// <param name="timeOn"> The time in second for the single stimulus to show </param>
        /// <param name="timeOff"> In case of a stimuli sequence, the time in seconds between two stimuli </param>
        IEnumerator Sequence(bool firstLeft, float timeOn, float timeOff)
        {
            RunStarted?.Invoke();
            Debug.Log("Sequence started!\nFirst ");
            yield return StartCoroutine(ShowStimulus(references[firstLeft ? 0 : 1], timeOn));
            yield return new WaitForSecondsRealtime(timeOff);
            Debug.Log("Second ");
            yield return StartCoroutine(ShowStimulus(Stimulus, timeOn));
            yield return new WaitForSecondsRealtime(timeOff);
            Debug.Log("Last ");
            yield return StartCoroutine(ShowStimulus(references[firstLeft ? 1 : 0], timeOn));
            canAnswer = true;
            RunFinished?.Invoke();
            //yield return new WaitForSecondsRealtime(timeOff);
            Debug.Log("Sequence Done!");

        }

        /// <inheritdoc/>
        protected override IEnumerator ShowStimulus(CylindricalCoordinates stimulus, float timeOn)
        {
            stimulus.gameObject.SetActive(true);
            Debug.Log("On at " + Time.unscaledTime);
            yield return new WaitForSecondsRealtime(timeOn);
            stimulus.gameObject.SetActive(false);
            Debug.Log("Off at " + Time.unscaledTime);
        }
    }


}