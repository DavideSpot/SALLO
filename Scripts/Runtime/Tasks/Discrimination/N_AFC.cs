using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SALLO
{
    /// <summary>
    /// Abstract <strong>Task</strong> component. Implements the answering logic for a generic N-alternatives forced-choice paradigm.
    /// </summary>
    /// <inheritdoc/>
    public abstract class N_AFC : Task
    {
        /// <inheritdoc/>
        protected override void GetAnswer<T>(T answer)
        {
            if (canAnswer)
            {
                Answer = answer.ToString();
                onParticipantAnswered.Invoke();
                canAnswer = false;
            }
        }
    }
}