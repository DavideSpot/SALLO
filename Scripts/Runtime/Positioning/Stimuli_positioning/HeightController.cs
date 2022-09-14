using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SALLO
{
    /// <summary>
    /// <strong>Stimuli Positioning</strong> component. Keep the height of a target GameObject at the same height as the GameObject with this component.
    /// </summary>
    public class HeightController : MonoBehaviour
    {
        /// <summary>
        /// The target GameObject
        /// </summary>
        public Transform taskPlaceHolder;
        // Start is called before the first frame update
        private void Start()
        {
            StartCoroutine(EvenHeight());
        }
        /// <summary>
        /// Update the target GameObject's height
        /// </summary>
        /// <returns></returns>
        IEnumerator EvenHeight()
        {
            yield return new WaitForEndOfFrame();
            taskPlaceHolder.position = new Vector3(taskPlaceHolder.position.x, transform.position.y, taskPlaceHolder.position.z);
        }
    }
}