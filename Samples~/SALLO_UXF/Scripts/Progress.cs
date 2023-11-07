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
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UXF;

public class Progress : MonoBehaviour
{
    bool paused = false;
    public GameObject PauseMenu;
    public Text TrialCounter;

    public UnityEvent OnPauseBegin;
    public UnityEvent OnPauseEnd;

    //private void Awake()
    //{
    //    DontDestroyOnLoad(this.gameObject);
    //    DontDestroyOnLoad(PauseMenu);
    //}

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) & paused == false)
        {
            TogglePause(PauseMenu);
            
        }
            
    }

    public void Quit()
    {
    #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
    #else
             Application.Quit();
    #endif
    }

    bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            OnPauseEnd?.Invoke();
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            OnPauseBegin?.Invoke();
            return (true);
        }
    }
    public void TogglePause(GameObject pauseMenu)
    {
        paused = togglePause();
        pauseMenu.SetActive(paused);
    }

    public void LoadNext()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void UpdateTrialCounter(Trial thisTrial)
    {
        TrialCounter.text = string.Format("Test paused at trial {0}/{1}", thisTrial.number, thisTrial.session.LastTrial.number);
    }

}