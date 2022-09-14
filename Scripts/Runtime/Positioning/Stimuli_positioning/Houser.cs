using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SALLO
{
    /// <summary>
    /// <strong>Stimuli Positioning</strong> component. Simplify changing reference frame for a target <see cref="GameObject"/> by choosing from a set of parent GameObjects.
    /// </summary>
    /// <remarks>The array of available parents is the array of children GameObjects assigned to the GameObject with this component.</remarks>
    public class Houser : MonoBehaviour
    {
        /// <summary>
        /// The target GameObject
        /// </summary>
        [HideInInspector]
        public Transform tenant;

        /// <summary>
        /// The current parent's index in the array of the available ones
        /// </summary>
        private int HomeIndex = 0;

        /// <summary>
        /// The current parent's <see cref="Transform"/>
        /// </summary>
        public Transform CurrentHouse { get
            {
                if (transform.childCount == 0)
                    throw new MissingReferenceException("This GameObject has no houses assigned.\nPlease set this GameObject as parent of at least one GameObject.");
                
                return transform.GetChild(HomeIndex);
            } 
        }
        /// <summary>
        /// Move the target GameObject to another parent
        /// </summary>
        /// <param name="homeIndex">the index of the desired parent in the array of the available ones</param>
        public void Relocate(int homeIndex)
        {
            if (homeIndex < 0)
                throw new ArgumentOutOfRangeException("The house index must be equal to or larger than 0");
            if (homeIndex >= transform.childCount)
                throw new ArgumentOutOfRangeException("The house index requested is larger than the available number of houses");
            tenant.SetParent(transform.GetChild(homeIndex));
            tenant.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            HomeIndex = homeIndex;
        }
    }
}