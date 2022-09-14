using System.Collections;
using UnityEngine;
//using UnityEditor;

namespace SALLO
{
    /// <summary>
    /// <strong>Task</strong> component. Implements the localization task
    /// </summary>
    public class Localization : Task
    {
        public GameObject Pointer;
        
        PositionWatcher StartingPoint;

        /// <summary>
        /// Decide whether to keep the stimulus on until the participant's answer
        /// </summary>
        public bool persistentStimulus = true;

        private void OnEnable()
        {
            //CheckPosition = false;

            Pointer.SetActive(false);
            Stimulus.gameObject.SetActive(false);
            canAnswer = false;

            StartingPoint = GetComponent<PositionWatcher>();
            RequestPositionCheck(CheckPosition);
            
            InputWatcher.OnMoreTriggersPressed += GetAnswer;
        }

        /// <inheritdoc/>
        protected override void GetAnswer<T>(T answer)
        {
            if (canAnswer)
            {
                float theta = StartingPoint.observer.eulerAngles.y % 360;
                Answer = (theta > 180 ? theta - 360 : theta).ToString("F1");
                if (persistentStimulus)
                {
                    Stimulus.gameObject.SetActive(false);
                    Debug.Log("Off at " + Time.unscaledTime);
                }
                onParticipantAnswered?.Invoke();
                canAnswer = false;
            }
        }
        private void OnDisable()
        {
            InputWatcher.OnMoreTriggersPressed -= GetAnswer;
        }

        /// <summary>
        /// Set the action to take when the target position is found
        /// </summary>
        protected override void PositionCheckSetUp()
        {
            StartingPoint.PositionFound = () =>
            {
                Pointer.SetActive(false);
                StartCoroutine(ShowStimulusAfter(new WaitForSecondsRealtime(TimeITI)));
            };
        }

        /// <inheritdoc/>
        public override void Run(float? timeOn = null, float? timeOff = null)
        {
            canAnswer = false;
            if (CheckPosition)
            {
                Stimulus.gameObject.SetActive(false);
                PositionCheckSetUp();
                Pointer.SetActive(true);
                StartingPoint.CheckAngle(transform.eulerAngles.y, 60);
            } else
            {
                StartCoroutine(ShowStimulusAfter( new WaitForSecondsRealtime(TimeITI) ));
            }
            

        }

        private IEnumerator ShowStimulusAfter(CustomYieldInstruction whatToWait, float? timeOn = null)
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
            if (!persistentStimulus)
            {
                yield return new WaitForSecondsRealtime(timeOn);
                stimulus.gameObject.SetActive(false);
                Debug.Log("Off at " + Time.unscaledTime);
            }
            canAnswer = true;
            RunFinished?.Invoke();
        }
    }
}