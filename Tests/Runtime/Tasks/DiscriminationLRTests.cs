using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SALLO.Tests
{
    public class DiscriminationLRTests
    {
        Transform observer;

        GameObject taskObject;
        DiscriminationLR task;

        [UnitySetUp]
        [OneTimeSetUp]
        public void SetUp()
        {
            observer = new GameObject("Main Camera").transform;
            observer.gameObject.AddComponent<AudioListener>();

            taskObject = Object.Instantiate(Resources.Load("Tasks/DiscriminationLR")) as GameObject;
            task = taskObject.GetComponent<DiscriminationLR>();
            task.RequestPositionCheck(false);
            task.sense = SensoryChannel.AUDIOVISUAL;
            task.TimeITI = .001f;
            task.TimeOn = .001f;
            task.TimeOff = .001f;
        }

        [UnityTest]
        public IEnumerator Run_triggerDuringSequence_ignore()
        {
            bool running = true;
            task.RunFinished = () => running = false;
            task.RunStarted = () => InputWatcher.OnOneTriggerPressed(triggers.none);
            task.Run();
            while (running)
                yield return null;
            Assert.That(task.Answer != "none");
            task.RunStarted = null;
            task.RunFinished = null;
            task.Answer = null;
        }

        [UnityTest]
        public IEnumerator Run_triggerAfterSequence_collect()
        {
            bool running = true;
            task.RunFinished = () => { running = false; InputWatcher.OnOneTriggerPressed(triggers.none); };
            yield return null;
            task.Run();
            while (running)
                yield return null;
            Assert.That(task.Answer == "none");
            task.RunStarted = null;
            task.RunFinished = null;
            task.Answer = null;
        }

        [UnityTest]
        public IEnumerator PositionCheckSetUp_InPosition_StartSequence()
        {
            bool sequenceStarted = false;
            task.RunStarted = () => sequenceStarted = true;

            task.RequestPositionCheck(true);
            yield return null;

            PositionWatcher pw = taskObject.GetComponent<PositionWatcher>();
            pw.PositionFound();

            for (int frames = 0; frames < 10; frames++)
                yield return null;

            Assert.That(sequenceStarted);
            task.RunStarted = null;
            task.RunFinished = null;
        }
    }
}
