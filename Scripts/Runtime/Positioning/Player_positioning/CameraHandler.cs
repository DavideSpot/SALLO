using UnityEngine;

namespace SALLO
{
    /// <summary>
    /// Handle the VR camera shifts
    /// </summary>
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField]
        Vector3 PositionCheck, RotationCheck;
        /// <summary>
        /// Counterbalance the VR <see cref="Camera"/> position shifts to keep it at the virtual space origin
        /// </summary>
        /// <remarks> Give the "Camera" GameObject a parent and update the parent position in the opposite direction of the camera position </remarks>
        void Update()
        {
            transform.parent.position = -transform.localPosition;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5);
        }
        /// <summary>
        /// display the global position and rotation
        /// </summary>
        private void LateUpdate()
        {
            PositionCheck = transform.position;
            RotationCheck = transform.eulerAngles;
        }
        /// <summary>
        /// Move the camera back to the virtual space origin
        /// </summary>
        public void Recenter()
        {
            transform.parent.SetPositionAndRotation(-transform.localPosition, Quaternion.Inverse(transform.localRotation));
        }
    }
}