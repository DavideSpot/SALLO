using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SALLO
{
    /// <summary>
    /// provides <see cref="GameObject"/>s with cylindrical coordinates - radius, azimuth (angle), elevation - instead of the cartesian ones used by <see href="Transform"></see>.
    /// </summary>
    [System.Serializable]
    public class CylindricalCoordinates : MonoBehaviour
    {
        [SerializeField]
        private float radius, azimuth, height;
        /// <summary>
        /// wrapper accessors. Update the desired coordinate properties via the method <see cref="ToTransform"/>
        /// </summary>
        public float Radius { get => radius; set => ToTransform(_radius: value); }
        public float Azimuth { get => azimuth; set => ToTransform(_azimuth: value); }
        public float Height { get => height; set => ToTransform(_height: value); }

        /// <summary>
        /// public method to update the <c>GameObject</c>'s transform according to the desired cylindrical coordinates
        /// </summary>
        /// <remarks>The new position is computed in the method <see cref="ToCartesian"/>. The new direction is the azimuth angle itself</remarks>
        /// <param name="_radius">the new radius</param>
        /// <param name="_azimuth">the new azimuth (angle)</param>
        /// <param name="_height">the new height (or elevation)</param>
        public void ToTransform(float? _radius = null, float? _azimuth = null, float? _height = null)
        {
            if (_radius.HasValue) radius = _radius.Value;
            if (_azimuth.HasValue) azimuth = _azimuth.Value;
            if (_height.HasValue) height = _height.Value;
            transform.SetLocalPositionAndRotation(
                ToCartesian(radius, azimuth, height), //position
                Quaternion.Euler(0f, azimuth, 0f)   //rotation
                );
        }
        /// <summary>
        /// The private method where conversion from cylindrical to cartesian coordinates happens
        /// </summary>
        /// <param name="_radius">the new radius</param>
        /// <param name="_azimuth">the new azimuth (angle)</param>
        /// <param name="_height">the new height (or elevation)</param>
        /// /// <returns>The position converted in cartesian coordinates</returns>
        Vector3 ToCartesian(float radius, float angle, float height)
        {
            return new Vector3(radius * Mathf.Sin(Mathf.Deg2Rad * angle), height, radius * (Mathf.Cos(Mathf.Deg2Rad * angle)));
        }
        /// <summary>
        /// Utility method to compute the distance from the camera a sphere <see cref="GameObject"/> should have to occlude the desired portion of field of view, in degrees.
        /// <seealso cref="PrimitiveType.Sphere"/>
        /// </summary>
        /// <param name="T">The shpere's transform component</param>
        /// <param name="stimulus_FoV_angle">the desired portion of field of view the shpere occludes</param>
        /// <returns>the distance from the origin of the virtual space's absolute reference frame</returns>
        public static float RadiusFromFoV(Transform T, float stimulus_FoV_angle) => (T.lossyScale.x / 2) / Mathf.Sin(Mathf.Deg2Rad * stimulus_FoV_angle / 2);

    }

    [CustomEditor(typeof(CylindricalCoordinates))]
    public class CylindricalCoordinatesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CylindricalCoordinates targetInEditor = (CylindricalCoordinates)target;
            if (GUILayout.Button("Update Position"))
            {
                targetInEditor.ToTransform();
            }
        }
    }
}