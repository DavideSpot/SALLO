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

using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SALLO.Tests
{
    public class TasksTests_prefabs
    {
        Object[] tasks;
        [OneTimeSetUp]
        public void SetUp()
        {
            tasks = Resources.LoadAll("Tasks");
        }

        [Test]
        public void TasksPrefabs_haveRequiredComponents()
        {
            GameObject task = null;
            bool ContainsAllComponents = true;
            foreach (Object _task in tasks)
            {
                task = Object.Instantiate(_task) as GameObject;
                if
                (
                !task.CompareTag("Task") |
                task.GetComponent<Task>() == null |
                task.GetComponent<ArrayPlacer>() == null |
                task.GetComponent<PositionWatcher>() == null
                )
                    ContainsAllComponents = false;
            }

            Assert.That(ContainsAllComponents);

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            foreach(Object _task in tasks)
                Object.DestroyImmediate(_task,true);
        }
    }
}
