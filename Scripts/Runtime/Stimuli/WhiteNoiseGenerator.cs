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

using UnityEngine;

namespace SALLO
{
    /// <summary>
    /// <strong>Stimuli</strong> component. Generates Uniform white noise procedurally, in real time.
    /// </summary>
    public class WhiteNoiseGenerator : MonoBehaviour
    {
        /// <summary>
        /// the noise generator. Change this to change the underlying noise distribution
        /// </summary>
        System.Random rand = new System.Random();
        /// <summary>
        /// The desired sampling rate
        /// </summary>
        public int samplerate = 44100;
        /// <summary>
        /// The audio source that plays the noise clip
        /// </summary>
        public AudioSource aud;
        /// <summary>
        /// The clip that stores the noise data
        /// </summary>
        AudioClip myClip;
        /// <summary>
        /// the clip (buffer) length
        /// </summary>
        public float soundLength;
        /// <summary>
        /// flag for sound start
        /// </summary>
        /// <remarks>if true, the buffer is filled with a logarithmic ramp</remarks>
        bool soundStart;

        /// <summary>
        /// Start emitting  white noise
        /// </summary>
        /// <remarks>
        /// Set <see cref="OnAudioRead"/> as <see cref="AudioClip.PCMReaderCallback"/>.
        /// <seealso href="https://docs.unity3d.com/ScriptReference/AudioClip.PCMReaderCallback.html"/>
        /// </remarks>
        private void OnEnable()
        {
            soundStart = true;
            if (myClip == null)
            {
                soundLength = GetComponentInParent<Task>().TimeOn;
                myClip = AudioClip.Create("WhiteNoise", (int)(soundLength * samplerate), 1, samplerate, false, OnAudioRead);
                aud.clip = myClip;

            }
            //aud.clip.LoadAudioData();
            aud.PlayOneShot(aud.clip);
        }

        //private void OnDisable()
        //{
        //    aud.Stop();
        //    //aud.clip.UnloadAudioData();
        //}

        /// <summary>
        /// fill the sound buffer with noise at every call of the <see cref="OnAudioRead"/> event.
        /// <seealso href="https://docs.unity3d.com/ScriptReference/AudioClip.PCMReaderCallback.html"/>
        /// <seealso href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnAudioFilterRead.html"/>
        /// </summary>
        /// <param name="data">the audio buffer to fill</param>
        void OnAudioRead(float[] data)
        {
            if (soundStart)
            {
                for (int i = 0; i < 45; i++)
                    data[i] = (float)(rand.NextDouble() * 2 - 1) * Mathf.Log(i + 1, 44);
                soundStart = false;
            }
            else
            {
                for (int i = 0; i < 45; i++)
                    data[i] = (float)(rand.NextDouble() * 2 - 1);
            }
            for (int i = 45; i < data.Length; i++)
                data[i] = (float)(rand.NextDouble() * 2 - 1);

        }
    }

}