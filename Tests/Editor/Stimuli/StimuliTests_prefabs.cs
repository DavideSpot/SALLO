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
    public class StimuliTests_prefabs
    {
        Object[] stimuli;

        [OneTimeSetUp]
        public void SetUp()
        {
            stimuli = Resources.LoadAll("Stimuli");
        }

        [Test]
        public void StimuliPrefabs_haveRequiredComponents()
        {
            GameObject stimulus = null;
            bool ContainsAllComponents = true;
            foreach (Object _stimulus in stimuli)
            {
                stimulus = Object.Instantiate(_stimulus) as GameObject;
                if
                (
                !stimulus.CompareTag("Stimulus") |
                stimulus.GetComponent<MeshRenderer>() == null |
                stimulus.GetComponent<AudioSource>() == null |
                stimulus.GetComponent<CylindricalCoordinates>() == null
                )
                    ContainsAllComponents = false;

                
            }

            Assert.That(ContainsAllComponents);

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            foreach (Object _stimulus in stimuli)
                Object.DestroyImmediate(_stimulus,true);
        }

    }
}
