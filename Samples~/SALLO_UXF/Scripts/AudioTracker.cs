using UnityEngine;
using System.Collections;
using System.IO;
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