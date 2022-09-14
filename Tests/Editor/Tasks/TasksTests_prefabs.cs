using System.Collections;
using System.Collections.Generic;
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
