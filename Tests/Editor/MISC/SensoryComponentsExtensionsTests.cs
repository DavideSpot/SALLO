using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace SALLO.Tests
{
    public class SensoryComponentsExtensionsTests
    {
        GameObject task = new GameObject();
        GameObject observer = new GameObject();
        
        [OneTimeSetUp]
        public void SetUp()
        {
            task.AddComponent<AudioSource>();
            new GameObject().AddComponent<AudioSource>().transform.SetParent(task.transform);

            observer.AddComponent<Camera>();
            observer.AddComponent<AudioListener>();
        }

        [Test]
        public void Visual_FalseRootTrueChild_disableRootRenderersOnly()
        {
            task.transform.GetChild(0).gameObject.Visual(true);
            task.Visual(false);

            Assert.That(
                task.GetComponents<Renderer>().All( (renderer) => renderer.enabled == false ) &
                task.transform.GetChild(0).GetComponents<Renderer>().All((renderer) => renderer.enabled == true)
                );

        }

        [Test]
        public void Acoustic_FalseRootTrueChild_disableRootRenderersOnly()
        {
            observer.Acoustic(false);
            task.transform.GetChild(0).gameObject.Acoustic(true);
            task.Acoustic(false);

            Assert.That(
                !observer.GetComponent<AudioListener>().enabled &
                task.GetComponents<AudioSource>().All((audio) => audio.enabled == false) &
                task.transform.GetChild(0).GetComponents<AudioSource>().All((audio) => audio.enabled == true)
                );

        }
    }
}
