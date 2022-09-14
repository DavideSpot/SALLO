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
    /// <strong>Task</strong> component. Implements the left-right discrimination task
    /// </summary>
    public class DiscriminationLR : N_AFC
    {

        public GameObject Pointer;
        
        private PositionWatcher StartingPoint;

        void OnEnable()
        {
/* Use for debugging:
            //UpdateDistance();
            //movingPositionsList = new float[] { -20, -10, 0, 10, 20 };
 */
            Stimulus.gameObject.SetActive(false);
            Pointer.SetActive(false);
            Transform BG = transform.GetChild(transform.childCount - 1);
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
                StartCoroutine(ShowStimulusAfter(new WaitForSecondsRealtime(TimeITI)));
            };
        }

        ///<inheritdoc/>
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
                StartCoroutine(ShowStimulusAfter(new WaitForSecondsRealtime(TimeITI), timeOn ?? TimeOn));
            }
        }

        public IEnumerator ShowStimulusAfter(CustomYieldInstruction whatToWait, float? timeOn = null)
        {
            yield return whatToWait;
            StartCoroutine(ShowStimulus(Stimulus, timeOn ?? TimeOn));
        }

        /// <inheritdoc/>
        protected override IEnumerator ShowStimulus(CylindricalCoordinates stimulus, float timeOn)
        {
            RunStarted?.Invoke();
            stimulus.gameObject.SetActive(true);
            Debug.Log("On at " + Time.unscaledTime);
            yield return new WaitForSecondsRealtime(timeOn);
            stimulus.gameObject.SetActive(false);
            canAnswer = true;
            Debug.Log("Off at " + Time.unscaledTime);
            RunFinished?.Invoke();
        }
    }
}