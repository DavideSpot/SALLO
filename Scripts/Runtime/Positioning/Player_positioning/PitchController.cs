using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SALLO
{
    /// <summary>
    /// <strong>Player Positioning</strong> component. Rules the pitch of a given sound according to the angular distance between the <c>GameObject</c> with this component and a reference direction
    /// </summary>
    public class PitchController : MonoBehaviour

    {
        /// <summary>
        /// the reference direction
        /// </summary>
        public Transform reference;
        /// <summary>
        /// the audio source providing the pitch controller
        /// </summary>
        private AudioSource source;
        /// <summary>
        /// the angular distance between this <c>GameObject</c>'s direction and the reference direction
        /// </summary>
        private float distance;
        /// <summary>
        /// the angular distance's direction
        /// </summary>
        private float sign;

        /// <summary>
        /// Search for the audio source::
        /// <list type="number">
        /// <item>in the last child in the hierarchy</item>
        /// <item>in the first GameObject tagged as "Background"</item>
        /// </list>
        /// if missing, throw an error
        /// </summary>
        private void OnEnable()
        {
            if (source == null)
            {
                source = transform.GetChild(transform.childCount - 1).GetComponent<AudioSource>();
                
                if (source == null)
                {
                    source = GameObject.FindGameObjectWithTag("Background").GetComponentInChildren<AudioSource>();
                    
                    if (source == null)
                        throw new MissingComponentException("no audio source for positional feedback found. Please add one to a background gameobject.");
                }
            }
            //if (source.clip != null)
                source.Play();
        }
        
        /// <summary>
        /// Compute the distance and update the pitch accordingly
        /// </summary>
        void LateUpdate()
        {
            sign = transform.InverseTransformDirection(reference.forward).z;
            distance = transform.InverseTransformDirection(reference.forward).x;
            //if (sign >= 0f)
            //{
            float maxPitch = 15f;
            if (distance != 0f)
            {
                source.pitch = Mathf.Clamp(1f / Mathf.Abs(distance), 0.1f, maxPitch);
            }
            else
            {
                source.pitch = maxPitch;
            }
            //}
            //else { source.pitch = 0.0f; }
        }
        private void OnDisable()
        {
            if (source != null)
                if (source.isPlaying)
                    source.Stop();
        }
    }
}