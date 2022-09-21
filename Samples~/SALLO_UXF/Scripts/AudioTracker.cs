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
using System.Linq;
using UXF;
public enum AudioType
{
    MONO,
    STEREO
}
/// <summary>
/// Attach this component to a gameobject and assign it in the trackedObjects field in an ExperimentSession to automatically record position/rotation of the object at each frame.
/// </summary>
public class AudioTracker : Tracker
{
    List<float[]> audioChunks = new List<float[]>();
    int? ChannelsNumber;
    /// <summary>
    /// Sets measurementDescriptor and customHeader to appropriate values
    /// </summary>
    ///
        
    protected override void SetupDescriptorAndHeader()
    {
        measurementDescriptor = "sound";
            
        customHeader = new string[]
        {
            "Channels_mixed",
            "Channels_number"
        };
    }
        
    protected override string[] GetCurrentValues()
    {
        string format = "0.####";
        string[] values = new string[] {
            string.Join(" ", audioChunks[0].Select(f=>f.ToString(format))),
            ChannelsNumber.ToString()
        };

        return values;
    }

    private void LateUpdate()
    {
        while(audioChunks.Count>0)
        {
            RecordRow();
            audioChunks.RemoveAt(0);
        }
    }

    //private void OnAudioFilterRead(float[] data, int channels)
    //{
    //    if (recording)
    //    {
    //        audioChunks.Add(data);
    //        if (ChannelsNumber == null)
    //            ChannelsNumber = channels;
    //    }
    //}
}