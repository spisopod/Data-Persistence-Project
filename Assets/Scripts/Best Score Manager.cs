using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization;

public class BestScoreManager : MonoBehaviour
{
    public static BestScoreManager Instance;
    public SaveData playerData;

    public int m_highestPoints;
    public string m_username;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // loads the best score if there was already one previously
            LoadBestScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public int highScore = 0;
        public string username = "";
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            m_username = data.username;
            m_highestPoints = data.highScore;
        }
        /*else
        {
            SaveData data = new();
        }*/
    }

    public void SaveBestScore()
    {
        SaveData data = new();
        data.username = m_username;
        data.highScore = m_highestPoints;

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", JsonUtility.ToJson(data));
    }
}
