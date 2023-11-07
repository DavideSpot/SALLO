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
using System.Collections.Generic;
using UnityEngine;
using UXF;
using UnityEngine.Events;
using System.Linq;
using SALLO;

public static class ListExtender
{
    /// <summary>
    /// extend List<int> to get elements' sum
    /// </summary>
    /// <param name="L">the caller list</param>
    /// <returns>the sum of the items in the list</returns>
    public static int Sum(this List<int> L)
    {
        int sum = 0;
        foreach (int el in L)
        {
            sum += el;
        }
        return sum;
    }
}

public class ExperimentController : MonoBehaviour
{
    private Session session;
    public BlockEventHandler B;
    public ArrayPlacer blockPositions;
    private ArrayPlacer stimuliPositions;
    private Task task;
    public Houser stimuliHouse;
    public GameObject observer;
    private PsyMethod psyMethod;

    private UnityAction EndTrial;
    private UnityAction SaveTrial;

    public void Generate(Session experimentSession)
    {
        session = experimentSession;
        // read type of experiment and instantiate corresponding prefab
        string experimentType = session.settings.GetString("experimentType");
        GameObject experimentObject = Instantiate(Resources.Load("Tasks/"+experimentType, typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        task = experimentObject.GetComponent<Task>();
        stimuliPositions = experimentObject.GetComponent<ArrayPlacer>();
        stimuliHouse.tenant = experimentObject.transform;
        string sensoryChannel = session.settings.GetString("sensoryChannel").ToUpper();
        task.GetSensoryChannel((SensoryChannel)Enum.Parse(typeof(SensoryChannel), sensoryChannel));
        task.RequestPositionCheck(true); //force observer to find a position


        //read stimuli parameters
        task.TimeOn = session.settings.GetFloat("time_on");
        task.TimeOff = session.settings.GetFloat("time_off");
        task.TimeITI = session.settings.GetFloat("time_ITI");

        // read in-block stimuli positioning parameters
        stimuliPositions.Radius = CylindricalCoordinates.RadiusFromFoV(
            stimuliPositions.transform, session.settings.GetFloat("fov_angle")
            );
        if (experimentType.ToLower() == "bisection")
            stimuliPositions.AzimuthBetween = (float)session.settings.GetInt("angle_between_references") / 2f;
        stimuliPositions.SetPositions();

        // read between-block positioning parameters
        int n_blocks = session.settings.GetInt("n_blocks");
        // adjust number of block reference frames and initialize them
        if (stimuliHouse.transform.childCount < n_blocks)
        {
        for (int i = stimuliHouse.transform.childCount; i < n_blocks;i++)
            (GameObject.Instantiate(Resources.Load("MISC/house")) as GameObject).transform.SetParent(stimuliHouse.transform);
        }
        if (stimuliHouse.transform.childCount > n_blocks)
        {
            for (int i = stimuliHouse.transform.childCount; i > n_blocks; i--)
                DestroyImmediate(stimuliHouse.transform.GetChild(stimuliHouse.transform.childCount - 1).gameObject);
        }
        blockPositions.MakeList();
        blockPositions.AzimuthBetween = (float)session.settings.GetInt("angle_between_blocks");
        blockPositions.SetPositions();

        //set trials' test values
        if (session.settings.baseDict.TryGetValue("procedure", out object procedure))
        {
            if (((string)procedure).Equals("QUEST"))
            {
                psyMethod = new pyQuest();
                //pyQuest.checkConvergence = session.settings.GetBool("checkConvergence");
                pyQuest.checkConvergence = false;
                if (session.settings.baseDict.ContainsKey("grain"))
                    pyQuest.grain = session.settings.GetFloat("grain");
                pyQuest.range = session.settings.GetInt("range");
                // testing angles here are anchor trials
                pyQuest.addNoise = session.settings.GetBool("addNoise");

                // use testingangles and testingtrials variables to set anchor trials
                psyMethod.testingAngles = new float[] { 0.1f, 0.9f }.Select(i => (i * pyQuest.range) - pyQuest.range / 2).ToList();
                psyMethod.testingTrials = new List<int> { 2, 2 };

                session.QuestSession(B, task, ref SaveTrial);
            }
        }
        else
        {
            psyMethod = new ConstantStimuli();
            psyMethod.testingAngles = session.settings.GetFloatList("testing_angles");
            psyMethod.testingTrials = session.settings.GetIntList("testing_trials");
        }
        //count trials per block and create blocks
        int numTrials = session.settings.GetIntList("testing_trials").Sum();

        session.GenerateBlocks(n_blocks, numTrials);

        foreach (Block bl in session.blocks)
        {
            bl.GenerateTrials(psyMethod.testingAngles, psyMethod.testingTrials);
        }
        if (task.CheckPosition)
            EndTrial = EndSafe;
        else
            EndTrial = EndUnsafe;

        B.onBlockBegin.AddListener(ChooseHouseFromBlock);

        if (session.settings.GetBool("jitter"))
            session.onTrialBegin.AddListener(JitterReferences);

        task.onParticipantAnswered.AddListener(EndTrial);

        if (task.CheckPosition)
            session.onTrialEnd.AddListener(ChooseHouseFromBlock);

        Invoke("StartTrial", task.TimeITI);
    }


    public void ChooseHouseFromBlock(Block block) => stimuliHouse.Relocate(block.settings.GetInt("rank"));
    public void ChooseHouseFromBlock(Trial trial) => stimuliHouse.Relocate(trial.block.settings.GetInt("rank"));
    public void JitterReferences(Trial trial)
    {
        int jitterAmplitude;
        if (trial.settings.baseDict.ContainsKey("maxjitter"))
        {
            jitterAmplitude = trial.settings.GetInt("maxjitter");
        }
        else
        {
            jitterAmplitude = Mathf.FloorToInt((observer.GetComponent<Camera>().fieldOfView - trial.settings.GetInt("fov_angle")) / 2f - stimuliPositions.AzimuthBetween) - 2;
        }
        stimuliPositions.Jitter(jitterAmplitude);
    }

    void StartTrial()
    {

        Debug.Log("Starting trial n° " + session.NextTrial.number.ToString());

        session.NextTrial.Begin2(B);
        task.Stimulus.Azimuth = stimuliPositions.AzimuthOffset + session.CurrentTrial.settings.GetFloat("movingAngle");
        task.Run();

    }

    private void RepeatTrial()
    {
        session.CurrentBlock.CreateTrial().settings.SetValue(
            "movingAngle", session.CurrentTrial.settings.GetFloat("movingAngle")
            );
    }
    private void SaveResults()
    {
        Debug.Log("Saving results...");
        session.CurrentTrial.result["ideal_block_angle"] = Angle.WrapTo180(stimuliHouse.CurrentHouse.localEulerAngles.y);
        session.CurrentTrial.result["true_block_angle"] = Angle.WrapTo180(task.transform.parent.eulerAngles.y);
        session.CurrentTrial.result["jitter"] = stimuliPositions.AzimuthOffset;
        float _stimulus = task.Stimulus.transform.localEulerAngles.y - stimuliPositions.AzimuthOffset;
        session.CurrentTrial.result["stimulus"] = Angle.WrapTo180(_stimulus);
        session.CurrentTrial.result["answer"] = task.Answer;
        SaveTrial?.Invoke();
    }
    private void EndAndPrepare()
    {
        Debug.Log("Ending trial...");
        session.CurrentTrial.End2(B);
        if (session.CurrentTrial == session.LastTrial)
        {
            session.End();
            B.onBlockBegin.RemoveAllListeners();
            session.onTrialBegin.RemoveAllListeners();
            task.onParticipantAnswered.RemoveListener(EndTrial);
            session.onTrialEnd.RemoveAllListeners();
            B.onBlockEnd.RemoveAllListeners();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }
        else
        {
            Invoke("StartTrial", task.TimeITI); //in time set from timeOff, invoke StartTrial method
        }
    }

    private void EndSafe()
    {
        if (!task.GetComponent<PositionWatcher>().PositionKept) RepeatTrial();
        else SaveResults();

        EndAndPrepare();
    }
    private void EndUnsafe()
    {
        SaveResults();
        EndAndPrepare();
    }
}





