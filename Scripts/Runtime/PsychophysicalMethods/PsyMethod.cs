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
using UnityEngine;

namespace SALLO
{
    /// <summary>
    /// Base class for any <strong>Psychophysical method</strong> component.
    /// </summary>
    /// <remarks> new psychophysical methods should inherit from this class</remarks>
    public abstract class PsyMethod
    {
        /// <summary>
        /// list of azimuth angles under investigation
        /// </summary>
        public List<float> testingAngles { get; set; }
        /// <summary>
        /// number of trials per each angle under investigation in <see cref="testingAngles"/>.
        /// </summary>
        public List<int> testingTrials { get; set; }

        /// <summary>
        /// Sequence of azimuth angles given the desired number of trials
        /// </summary>
        public List<float> TrialSequence { get; set; }

        /// <summary>
        /// Generate the trial sequence, given the angles under investigation and the number of trials per each angle
        /// </summary>
        /// <returns>The trial sequence</returns>
        public List<float> GetTrialSequence()
        {
            if (TrialSequence != null)
                TrialSequence = new List<float>();
            for (int i = 0; i < testingAngles.Count; i++)
            {
                for (int j = 0; j < testingTrials[i]; j++)
                {
                    TrialSequence.Add(testingAngles[i]);
                }
            }
            TrialSequence.shuffle();
            return TrialSequence;
        }
    }

    /// <summary>
    /// Extension class to shuffle the items in a list
    /// </summary>
    public static class IListShuffler
    {
        /// <summary>
        /// shuffle the items in a list
        /// </summary>
        public static void shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}