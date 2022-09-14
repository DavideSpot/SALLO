using System.Collections;
using System;
using NUnit.Framework;
using UnityEngine;
using SALLO;
using UnityEngine.TestTools;

namespace SALLO.Tests
{
    public class PositionWatcherTests
    {

        GameObject taskObject;
        
        PositionWatcher watcher;

        Transform observer;

        [UnitySetUp]
        [OneTimeSetUp]
        public void SetUp()
        {
            taskObject = new GameObject();

            DummyTask task = taskObject.AddComponent<DummyTask>();
            //task.CheckPosition = false;
            //task.sense = SensoryChannel.PROPRIOCEPTIVE;

            GameObject stimulusObject = new GameObject();
            stimulusObject.transform.SetParent(taskObject.transform);
            stimulusObject.AddComponent<AudioSource>();

            taskObject.AddComponent<PitchController>();

            observer = new GameObject("Main Camera").transform;
            observer.gameObject.AddComponent<AudioListener>();

            watcher = taskObject.AddComponent<PositionWatcher>();
            watcher.observer = observer;
        }

        [UnityTest]
        public IEnumerator CheckAngle_TestAndObserverAngles_trueIfInRange(
            [Values(45f)] float observerAngle,
            [Values(45f,0f)] float testAngle,
            [Random(-360f, 360f, 1)] float observerXrot,
            [Random(-360f, 360f, 1)] float observerZrot,
            [Values(10)] int frameLimit)
        {
            observer.SetPositionAndRotation(Vector3.zero,Quaternion.Euler(observerXrot,observerAngle,observerZrot));

            Time.timeScale = 50f;

            watcher.CheckAngle(testAngle, frameLimit);

            float frames = 0;
            while (frames <= frameLimit+5)
            {
                frames++;
            yield return null;
            }

            Time.timeScale = 1.0f;

            if (testAngle == observerAngle)
                Assert.That(watcher.inPosition);
            if (testAngle != observerAngle)
                Assert.That(!watcher.inPosition);
        }

        private IEnumerator IsKeepingPosition_rotateObserver(float startAngle, float endAngle, float tolerance)
        {
            observer.rotation = Quaternion.Euler(0f, startAngle, 0f);
            taskObject.transform.rotation = observer.rotation;

            Time.timeScale = 100f;

            int frames = 0;
            while( watcher.IsKeepingPosition(tolerance).MoveNext() & frames <= (int)tolerance)
            {
                observer.Rotate(Vector3.up, 1f);
                frames++;
                yield return null;
            }

            Time.timeScale = 1f;

            if (watcher.observer_stimuli_angle >= tolerance)
                Assert.That(!watcher.PositionKept);
            if (watcher.observer_stimuli_angle < tolerance)
                Assert.That(watcher.PositionKept);
        }

        [UnityTest]
        public IEnumerator IsKeepingPosition_rotateObserver_NotKept([Random(-360f,360f,2)]float startAngle, [Random(-360f, 360f, 2)] float endAngle)
        {
            float tolerance = Math.Abs(endAngle-startAngle) - 1f;
            return IsKeepingPosition_rotateObserver(startAngle, endAngle, tolerance);
        }

        [UnityTest]
        public IEnumerator IsKeepingPosition_rotateObserver_Kept([Random(-360f, 360f, 2)] float startAngle, [Random(-360f, 360f, 2)] float endAngle)
        {
            float tolerance = Math.Abs(endAngle - startAngle) + 1f;
            return IsKeepingPosition_rotateObserver(startAngle, endAngle, tolerance);
        }

        //[UnityTearDown]
        //public void TearDown()
        //{
        //    Time.timeScale = 1f;
        //}
    }
}
