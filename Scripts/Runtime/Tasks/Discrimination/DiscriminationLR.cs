using System.Collections;
using UnityEngine;
//using UnityEditor;
using UnityEngine.Events;

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