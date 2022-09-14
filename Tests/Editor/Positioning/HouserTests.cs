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
