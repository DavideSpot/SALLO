using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SALLO.Tests
{
    public class StimuliTests_prefabs
    {
        Object[] stimuli;

        [OneTimeSetUp]
        public void SetUp()
        {
            stimuli = Resources.LoadAll("Stimuli");
        }

        [Test]
        public void StimuliPrefabs_haveRequiredComponents()
        {
            GameObject stimulus = null;
            bool ContainsAllComponents = true;
            foreach (Object _stimulus in stimuli)
            {
                stimulus = Object.Instantiate(_stimulus) as GameObject;
                if
                (
                !stimulus.CompareTag("Stimulus") |
                stimulus.GetComponent<MeshRenderer>() == null |
                stimulus.GetComponent<AudioSource>() == null |
                stimulus.GetComponent<CylindricalCoordinates>() == null
                )
                    ContainsAllComponents = false;

                
            }

            Assert.That(ContainsAllComponents);

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            foreach (Object _stimulus in stimuli)
                Object.DestroyImmediate(_stimulus,true);
        }

    }
}
