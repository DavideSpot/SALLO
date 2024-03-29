﻿/*  Copyright (C) 2022 Unit for Visually Impaired People (UVIP) - Fondazione Istituto Italiano di Tecnologia (IIT)
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
using System;
using UnityEngine;
using UnityEngine.Events;
using UXF;
using SALLO;

public static class UXFExtensions
{
    // extend session to create list of blocks according to a parameter in settings file
    public static void GenerateBlocks(this Session S, int numBlocks, int? numTrials)
    {
        for (int i = 0; i < numBlocks; i++)
        {
            if (numTrials.HasValue) { S.CreateBlock((int)numTrials); } else { S.CreateBlock(); }
            S.blocks[i].settings.SetValue("rank", i);

        }
        S.blocks.Shuffle();
    }

    //extend block to specify moving angle positions and their repetitions
    public static void GenerateTrials(this Block block, List<float> _testingAngles, List<int> _testingTrials)
    {
        List<Trial>.Enumerator trialEnum = block.trials.GetEnumerator();
        trialEnum.MoveNext();
        for (int i = 0; i < _testingAngles.Count; i++)
        {
            for (int j = 0; j < _testingTrials[i]; j++)
            {
                trialEnum.Current.settings.SetValue("movingAngle", _testingAngles[i]);
                trialEnum.MoveNext();
            }
        }
        // shuffle the trial order, so the catch trials are in random positions
        block.trials.Shuffle();
    }

    public static void QuestSession(this Session session, BlockEventHandler B, Task task, ref UnityAction SaveAction)
    {

        B.onBlockBegin.AddListener((Block b) => pyQuest.restart_quest());
        session.onTrialBegin.AddListener((Trial t) =>
        {
            if (t.settings.baseDict.ContainsKey("movingAngle"))
                t.settings.SetValue("updateQuest", false);
            else
            { //check if value already set. if not, add pyQuest value
                t.settings.SetValue("movingAngle", pyQuest.query_quest());
                t.settings.SetValue("updateQuest", true);
            }

        });
        SaveAction = () =>
        {
            if (session.CurrentTrial.settings.GetBool("updateQuest"))
                pyQuest.update_quest(task.Answer.Equals(triggers.right.ToString()) ? 1 : 0);
        };

        session.onSessionEnd.AddListener((Session s) => pyQuest.StopPy());
        pyQuest.Converged = () => { session.CurrentBlock.trials.RemoveRange(session.CurrentTrial.numberInBlock, session.CurrentBlock.trials.Count - session.CurrentTrial.numberInBlock); };

        pyQuest.LaunchPy();
    }

    public static void Begin2(this Trial T,BlockEventHandler B)
    {

        if (T.numberInBlock == 1)
        {
            Debug.Log("Starting block n° " + T.block.number.ToString());
            B.onBlockBegin?.Invoke(T.block);
        }
            
        T.Begin();
    }

    public static void End2(this Trial T,BlockEventHandler B)
    {
        T.End();
        if (T.numberInBlock == T.block.trials.Count) B.onBlockEnd?.Invoke(T.block);
    }

}

/// <summary>
/// Event containing a Block as a parameter 
/// </summary>
[Serializable]
public class BlockEvent : UnityEvent<Block>
{

}