using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SALLO
{
    /// <summary>
    /// Base class for any <strong>Task</strong> component.
    /// </summary>
    /// <remarks> new tasks should inherit from this class</remarks>
    [RequireComponent(typeof(ArrayPlacer))]
    [RequireComponent(typeof(PositionWatcher))]
    public abstract class Task : MonoBehaviour
    {
        /// <summary>
        /// The perceptual rendering of the task's stimuli
        /// </summary>
        public SensoryChannel sense;
        
        /// <summary>
        /// Decide whether to deliver the stimuli according to the participant's orientation
        /// </summary>
        public bool CheckPosition { get; protected set; }

        /// <summary>
        /// The stimulus.
        /// </summary>
        public CylindricalCoordinates Stimulus;
        
        /// <summary>
        /// The time in second for the single stimulus to show
        /// </summary>
        public float TimeOn;

        /// <summary>
        /// In case of a stimuli sequence, the time in seconds between two stimuli
        /// </summary>
        public float TimeOff;

        /// <summary>
        /// inter trial interval, in seconds
        /// </summary>
        public float TimeITI;

        /// <summary>
        /// The particpant's answer
        /// </summary>
        public string Answer;

        /// <summary>
        /// Decide whetther to collect the participant's answer
        /// </summary>
        protected bool canAnswer = false;

        /// <summary>
        /// Event triggered from the participant having answered
        /// </summary>
        public UnityEvent onParticipantAnswered = new UnityEvent();

        /// <summary>
        /// Action performed as the stimulis sequence delivery starts
        /// </summary>
        public UnityAction RunStarted;

        /// <summary>
        /// Action performed as the stimulis sequence delivery starts
        /// </summary>
        public UnityAction RunFinished;


        /// <summary>
        /// The answer collection logic for the given task
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="answer"></param>
        protected abstract void GetAnswer<T>(T answer = default);

        /// <summary>
        /// Starts of the procedures that ends with the stimulus/stimuli sequence delivery
        /// </summary>
        /// <param name="timeOn"> The time in second for the single stimulus to show </param>
        /// <param name="timeOff"> In case of a stimuli sequence, the time in seconds between two stimuli </param>
        public abstract void Run(float? timeOn = null, float? timeOff = null);

        /// <summary>
        /// implements the appearance of a single stimulus delivery
        /// </summary>
        /// <param name="stimulus"> The stimulus to show </param>
        /// <param name="timeOn"> The time in second for the stimulus to show </param>
        /// <returns></returns>
        protected abstract IEnumerator ShowStimulus(CylindricalCoordinates stimulus, float timeOn);

        /// <summary>
        /// Method to control the stimuli acoustic rendering behavior
        /// </summary>
        /// <param name="isOn">Decide whether to rendering the stimuli acoustically</param>
        /// <param name="root">The GameObject acting as parent of all the stimuli GameObjects of interest.</param>
        public virtual void Acoustic(bool isOn, Transform root = null)
        {
            if (root == null)
                root = transform;

            foreach (Transform child in root)
            {
                if (child.childCount > 0)
                    Acoustic(isOn, child);
                child.gameObject.Acoustic(isOn);
            }
        }

        /// <summary>
        /// Method to control the stimuli visual rendering behavior
        /// </summary>
        /// <param name="isOn">Decide whether to rendering the stimuli visually</param>
        /// <param name="root">The GameObject acting as parent of all the stimuli GameObjects of interest.</param>
        public virtual void Visual(bool isOn, Transform root = null)
        {
            if (root == null)
                root = transform;

            foreach (Transform child in root)
            {
                if (child.childCount > 0)
                    Visual(isOn, child);
                child.gameObject.Visual(isOn);
            }
        }

        /// <summary>
        /// Rule the stimuli audio-visual rendering behavior.
        /// </summary>
        /// <param name="sensoryChannel">The desired perceptual stimulation</param>
        public void GetSensoryChannel(SensoryChannel sensoryChannel)
        {
            sense = sensoryChannel;
            switch (sense)
            {
                case SensoryChannel.VISUAL:
                    this.Visual(true); this.Acoustic(false);
                    break;
                case SensoryChannel.ACOUSTIC:
                    this.Visual(false); this.Acoustic(true);
                    break;
                case SensoryChannel.AUDIOVISUAL:
                    this.Visual(true); this.Acoustic(true);
                    break;
                case SensoryChannel.PROPRIOCEPTIVE:
                    this.Visual(true); this.Acoustic(false);
                    break;
            }
        }
        /// <summary>
        /// Control whether the position check has been requested. If so, set it up accordingly
        /// </summary>
        /// <param name="checkPosition">Decide whether to check the position</param>
        /// <remarks>
        /// The <see cref="PositionWatcher"/> component attached to the Task GameObject prefab should be disabled in the editor
        /// </remarks>
        public void RequestPositionCheck(bool checkPosition)
        {
            
            CheckPosition = checkPosition;
            GetComponent<PositionWatcher>().enabled = CheckPosition;
            if (checkPosition)
                PositionCheckSetUp();

        }

        /// <summary>
        /// Perform the necessary actions to check the position
        /// </summary>
        protected abstract void PositionCheckSetUp();

    }
}