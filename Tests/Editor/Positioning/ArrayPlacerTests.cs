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

using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SALLO.Tests
{
   [TestFixture(5)]
    public class ArrayPlacerTests
    {
        int Nchildren;

        GameObject root;
        List<GameObject> children;

        ArrayPlacer childrenPlacer;

        public ArrayPlacerTests(int nchildren)
        {
            Nchildren = nchildren;
        }

        [SetUp]
        public void TestSetUp()
        {
            //int Nchildren = 5;
            root = new GameObject();
            children = new List<GameObject>();
            childrenPlacer = root.AddComponent<ArrayPlacer>();

            for (int i = 0; i < Nchildren; i++)
            {
                GameObject newChild = GameObject.Instantiate(Resources.Load("Stimuli/stimulus"), new Vector3(0, 0, 0), Quaternion.identity, root.transform) as GameObject;
                //GameObject newChild = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                newChild.AddComponent<CylindricalCoordinates>();
                newChild.transform.SetParent(root.transform);

                if (i % 2 == 0)
                    newChild.tag = "even";
                else
                    newChild.tag = "odd";

                children.Add(newChild);

            }

        }

        [TestCase(true)]
        [TestCase(false)]
        public void MakeList_OddKey_isexcluded(bool exclude)
        {
            CylindricalCoordinates[] childrenCylCrd = root.GetComponentsInChildren<CylindricalCoordinates>();
            List<CylindricalCoordinates> IncludedChildrenCylCrd = new List<CylindricalCoordinates>();
            for (int i = 0; i < root.transform.childCount; i++)
            {
                if (i % 2 == 0)
                    IncludedChildrenCylCrd.Add(childrenCylCrd[i]);
                else if (!exclude)
                    IncludedChildrenCylCrd.Add(childrenCylCrd[i]);
            }

            List<CylindricalCoordinates> selectedChildrenCylCrd = childrenPlacer.MakeList(new string[] { "odd" }, exclude);

            Assert.That(true,"IncludedChildrenCylCrd.SequenceEqual(selectedChildrenCylCrd)");

        }

    }
}
