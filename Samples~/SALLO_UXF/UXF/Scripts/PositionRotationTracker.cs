using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace UXF
{
    public enum CoordinatesType
    {
        GLOBAL,
        LOCAL
    }
    /// <summary>
    /// Attach this component to a gameobject and assign it in the trackedObjects field in an ExperimentSession to automatically record position/rotation of the object at each frame.
    /// </summary>
    public class PositionRotationTracker : Tracker
    {
        /// <summary>
        /// Sets measurementDescriptor and customHeader to appropriate values
        /// </summary>
        ///
        [SerializeField]
        public CoordinatesType coords; 
        protected override void SetupDescriptorAndHeader()
        {
            measurementDescriptor = "movement";
            
            customHeader = new string[]
            {
                "pos_x",
                "pos_y",
                "pos_z",
                "rot_x",
                "rot_y",
                "rot_z"
            };
        }

        /// <summary>
        /// Returns current position and rotation values
        /// </summary>
        /// <returns></returns>
        protected override string[] GetCurrentValues()
        {
            Vector3 p = new Vector3();
            Vector3 r = new Vector3();
            // get position and rotation
            if (coords == CoordinatesType.GLOBAL)
            {
                p = gameObject.transform.position;
                r = gameObject.transform.eulerAngles;
            }
            else if (coords == CoordinatesType.LOCAL)
            {
                p = gameObject.transform.localPosition;
                r = gameObject.transform.localEulerAngles;
            }

            string format = "0.####";

            // return position, rotation (x, y, z) as an array
            var values =  new string[]
            {
                p.x.ToString(format),
                p.y.ToString(format),
                p.z.ToString(format),
                r.x.ToString(format),
                r.y.ToString(format),
                r.z.ToString(format)
            };

            return values;
        }
    }
}