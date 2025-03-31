using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;
using System;
using UnityEngine.SocialPlatforms.Impl;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    public InputField input;
    public Text highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        // once player is done, input field equals that username now
        input.onEndEdit.AddListener(SubmitName);

        BestScoreManager.Instance.LoadBestScore();
        highScoreText.text = $"Best Score : {BestScoreManager.Instance.m_username} : {BestScoreManager.Instance.m_highestPoints}";
    }

    private void SubmitName(string arg0)
    {
        // BestScoreManager script gets the user-inputted name as username
        BestScoreManager.Instance.m_username = input.text;
    }

    // loads game scene
    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    // quits game
    public void QuitButton()
    {
        #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
        #else
                Application.Quit();
        #endif
    }
}
