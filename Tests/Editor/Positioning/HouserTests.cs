using System.Collections;
using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SALLO.Tests
{
    [TestFixture(7)]
    public class HouserTests
    {
        int Nhouses;

        GameObject houserObject;
        Houser houserComponent;

        public HouserTests(int nhouses)
        {
            Nhouses = nhouses;
        }

        [SetUp]
        public void TestSetUp()
        {
            houserObject = new GameObject("houser");
            houserComponent = houserObject.AddComponent<Houser>();

            for (int i = 0; i < Nhouses; i++)
            {
                GameObject house = new GameObject("house");
                house.transform.SetParent(houserObject.transform);
            }

            GameObject tenant = new GameObject("tenant");
            houserComponent.tenant = tenant.transform;
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(7)]
        [TestCase(9)]
        public void Relocate_int_housechangedifexists(int houseIndex)
        {
            if (houseIndex < 0 | houseIndex >= Nhouses)
                Assert.Throws<ArgumentOutOfRangeException>(() => houserComponent.Relocate(houseIndex));
            else
            {
                houserComponent.Relocate(houseIndex);
                Assert.That(houserComponent.tenant.parent, Is.EqualTo(houserObject.transform.GetChild(houseIndex)) );
                Assert.That(houserComponent.tenant.parent, Is.EqualTo(houserComponent.CurrentHouse));
            }
        }
    }
}
