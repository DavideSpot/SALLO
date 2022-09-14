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
using System.Linq;

namespace SALLO.Tests
{
    public class SensoryComponentsExtensionsTests
    {
        GameObject task = new GameObject();
        GameObject observer = new GameObject();
        
        [OneTimeSetUp]
        public void SetUp()
        {
            task.AddComponent<AudioSource>();
            new GameObject().AddComponent<AudioSource>().transform.SetParent(task.transform);

            observer.AddComponent<Camera>();
            observer.AddComponent<AudioListener>();
        }

        [Test]
        public void Visual_FalseRootTrueChild_disableRootRenderersOnly()
        {
            task.transform.GetChild(0).gameObject.Visual(true);
            task.Visual(false);

            Assert.That(
                task.GetComponents<Renderer>().All( (renderer) => renderer.enabled == false ) &
                task.transform.GetChild(0).GetComponents<Renderer>().All((renderer) => renderer.enabled == true)
                );

        }

        [Test]
        public void Acoustic_FalseRootTrueChild_disableRootRenderersOnly()
        {
            observer.Acoustic(false);
            task.transform.GetChild(0).gameObject.Acoustic(true);
            task.Acoustic(false);

            Assert.That(
                !observer.GetComponent<AudioListener>().enabled &
                task.GetComponents<AudioSource>().All((audio) => audio.enabled == false) &
                task.transform.GetChild(0).GetComponents<AudioSource>().All((audio) => audio.enabled == true)
                );

        }
    }
}
