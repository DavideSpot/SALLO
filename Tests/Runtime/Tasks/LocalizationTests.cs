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
using UnityEngine.Events;
using UnityEngine.TestTools;

namespace SALLO.Tests
{
    public class LocalizationTests
    {
        Transform observer;

        GameObject taskObject;
        Localization task;

        [UnitySetUp]
        [OneTimeSetUp]
        public void SetUp()
        {
            observer = new GameObject("Main Camera").transform;
            observer.gameObject.AddComponent<AudioListener>();

            taskObject = Object.Instantiate(Resources.Load("Tasks/Localization")) as GameObject;
            task = taskObject.GetComponent<Localization>();
            task.RequestPositionCheck(false);
            task.persistentStimulus = true;
            task.sense = SensoryChannel.AUDIOVISUAL;
            task.TimeITI = .01f;
            task.TimeOn = .01f;
            task.TimeOff = .01f;
        }

        [UnityTest]
        public IEnumerator Run_triggerBetweenTwoStimuli_ignore()
        {
            
            task.RunFinished = () => InputWatcher.OnMoreTriggersPressed(triggers.none);
            
            bool running = true;

            UnityAction clearAnswer = () => { running = false; task.Answer = null; };
            task.onParticipantAnswered.AddListener( clearAnswer );

            task.Run();
            while (running)
                yield return null;
            
            InputWatcher.OnMoreTriggersPressed(triggers.none);
            Assert.That(task.Answer == null);
            
            task.onParticipantAnswered.RemoveListener( clearAnswer );
        }

        [UnityTest]
        public IEnumerator Run_triggerAfterStimulusShow_collect()
        {
            bool running = true;
            task.RunFinished = () => { running = false; InputWatcher.OnMoreTriggersPressed(triggers.none); };

            task.Run();
            while (running)
                yield return null;

            float theta = observer.eulerAngles.y % 360;
            Assert.That(task.Answer == (theta > 180 ? theta - 360 : theta).ToString("F1"));

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

        [TearDown]
        public void TearDown()
        {
            task.RunStarted = null;
            task.RunFinished = null;
            task.Answer = null;
        }
    }
}
