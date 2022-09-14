using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SALLO
{
    /// <summary>
    /// <strong>Stimuli Positioning</strong> component. Match the position of a target <see cref="GameObject"/>
    /// </summary>
    public class PositionMatcher : MonoBehaviour
    {
        /// <summary>
        /// The target GameObject
        /// </summary>
        public Transform master;

        /// <summary>
        /// Match the position at every unity Update
        /// </summary>
        void Update()
        {
            transform.position = master.position;
        }
    }
}