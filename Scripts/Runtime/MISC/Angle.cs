using System.Collections;
using UnityEngine;

namespace SALLO
{
    /// <summary>
    /// Utility class to change format to Unity rotations 
    /// </summary>
    public static class Angle
    {
        /// <summary>
        /// Converts the angle range from [0,360] to [-180,180].
        /// </summary>
        /// <param name="angle">The angle in [0,360] format</param>
        /// <returns>The angle in [-180,180] format</returns>
        public static dynamic WrapTo180(dynamic angle)
        {
            float theta = angle % 360f;
            return theta > 180 ? theta - 360 : theta;
        }
    }
}
