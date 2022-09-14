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
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SALLO.Tests
{
    public class NeckProprioceptionTests
    {
        Transform observer;

        GameObject taskObject;
        NeckProprioception task;

        [UnitySetUp]
        [OneTimeSetUp]
        public void SetUp()
        {
            observer = new GameObject("Main Camera").transform;

            taskObject = Object.Instantiate(Resources.Load("Tasks/Proprioception")) as GameObject;
            task = taskObject.GetComponent<NeckProprioception>();
            task.sense = SensoryChannel.AUDIOVISUAL;
            task.TimeITI = .01f;
            task.TimeOn = .01f;
            task.TimeOff = .01f;
        }

        [UnityTest]
        public IEnumerator FindStimulus_NotFound_onlyRunStarted()
        {
            task.RequestPositionCheck(false);

            bool sequenceStarted = false;
            task.RunStarted = () => sequenceStarted = true;

            bool sequenceFinished = false;
            task.RunFinished = () => sequenceFinished = true;

            task.Run();

            for (int frames = 0; frames < 10; frames++)
                yield return null;

            Assert.That(sequenceStarted & !sequenceFinished);
        }

        [UnityTest]
        public IEnumerator FindStimulus_Found_RunStartedAndFinished()
        {
            task.RequestPositionCheck(false);

            bool sequenceStarted = false;
            task.RunStarted = () => sequenceStarted = true;

            bool sequenceFinished = false;
            task.RunFinished = () => sequenceFinished = true;

            task.Run();
            yield return null;

            PositionWatcher pw = taskObject.GetComponent<PositionWatcher>();
            pw.PositionFound();

            for (int frames = 0; frames < 10; frames++)
                yield return null;

            Assert.That(sequenceStarted & sequenceFinished);
        }
    }
}
