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
using UnityEngine;

namespace SALLO
{
    public enum SensoryChannel
    {
        VISUAL, ACOUSTIC, AUDIOVISUAL, PROPRIOCEPTIVE
    }
    /// <summary>
    /// Extension methods for the <see cref="GameObject"/> class, used to access their components related to acoustic and visual stimuli.
    /// <para>
    /// See also 
    /// <seealso cref="ResonanceAudioBehaviour"/>; 
    /// <seealso cref="Task"/>
    /// </para>
    /// </summary>
    public static class SensoryGameObject
    {
        /// <summary>
        /// rules whether the calling <c"GameObject"/> is visible
        /// </summary>
        /// <remarks><term>Visual</term> <description>rules the enabling of all the <see cref="Renderer"/> components in the <c"GameObject"/>.</description></remarks>
        /// <param name="G">The calling GameObject</param>
        /// <param name="isOn">the desired enabling state</param>
        public static void Visual(this GameObject G, bool isOn)
        {
            Renderer r = G.GetComponent<Renderer>();
            if (r != null)
                r.enabled = isOn;
        }
        /// <summary>
        /// rules whether the calling <c"GameObject"/> is audible
        /// </summary>
        /// <remarks>
        /// <term>Acoustic</term><description>rules the enabling of all the <see cref="AudioBehaviour"/> components in the <c"GameObject"/></description>.
        /// <para>
        /// COMPATIBILITY ISSUE:
        /// resonance audio package <see href="https://docs.unity3d.com/Packages/com.unity.google.resonance.audio@2.0/manual/index.html"/> is empty. dependency is useless
        /// Issues:
        /// <list type="bullet">
        /// <item>cannot read resonanceaudio API <see href="https://resonance-audio.github.io/resonance-audio/develop/unity/getting-started.html">  from package</item>
        /// <item>cannot read plugins stored in assets (referenced to assembly-CSharp from custom package, it would be a cyclical reference)</item>
        /// </list>
        /// </para>
        /// <para>
        /// WORKAROUND:
        /// created middleware <see cref="ResonanceAudioBehaviour"/> abstract class.
        /// <list type="number">
        /// <item>Import ResonanceAudio as asset <see href="https://resonance-audio.github.io/resonance-audio/develop/unity/getting-started.html"></item>
        /// <item> edit the ResonanceAudio classes to make them inherit from ResonanceAudioBehaviour instead of MonoBehaviour.
        /// <example>
        /// <code>
        /// public class ResonanceAudioListener : <strike>MonoBehaviour</strike> ResonanceAudioBehaviour {
        /// </code>
        /// </example>
        /// </item>
        /// </list>
        /// Do it at least for ResonanceAudioListener & ResonanceAudioSource
        /// </para>
        /// </remarks>
        /// <param name="G">The calling GameObject</param>
        /// <param name="isOn">the desired enabling state</param>
        public static void Acoustic(this GameObject G, bool isOn)
        {
            foreach (AudioBehaviour audio in G.GetComponents<AudioBehaviour>())
                audio.enabled = isOn;
#if RESONANCEPLUGIN_ISPRESENT // symbol defined in "it.iit.sallo.scripts.runtime" assembly definintion's "version defines" field 
 
            foreach (ResonanceAudioBehaviour audio in G.GetComponents<ResonanceAudioBehaviour>())
             audio.enabled = isOn;
#endif

        }
    }

}