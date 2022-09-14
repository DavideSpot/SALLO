using System.Collections;
using System.Collections.Generic;
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