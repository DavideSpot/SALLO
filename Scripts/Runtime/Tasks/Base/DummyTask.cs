using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SALLO
{
    /// <summary>
    /// Dummy Task class. Needed for unit testing. Please ignore
    /// </summary>
    public class DummyTask : Task
    {
        public override void Run(float? timeOn = null, float? timeOff = null)
        {
            throw new System.NotImplementedException();
        }

        protected override void GetAnswer<T>(T answer = default)
        {
            throw new System.NotImplementedException();
        }

        protected override void PositionCheckSetUp()
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerator ShowStimulus(CylindricalCoordinates stimulus, float timeOn)
        {
            throw new System.NotImplementedException();
        }
    }
}
