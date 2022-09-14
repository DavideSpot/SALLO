/*  Copyright (C) 2022 Unit for Visually Impaired People (UVIP) - Fondazione Istituto Italiano di Tecnologia (IIT)
    Author: Davide Esposito
    email: davide.esposito@iit.it | spsdvd48@gmail.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SALLO.Tests
{
    public class BisectionTests
    {
        Transform observer;

        GameObject taskObject;
        Bisection task;

        [UnitySetUp]
        [OneTimeSetUp]
        public void SetUp()
        {
            observer = new GameObject("Main Camera").transform;
            observer.gameObject.AddComponent<AudioListener>();

            taskObject = Object.Instantiate(Resources.Load("Tasks/Bisection")) as GameObject;
            task = taskObject.GetComponent<Bisection>();
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
            task.RunStarted = ()=>InputWatcher.OnOneTriggerPressed(triggers.none);
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
        }
    }
}
