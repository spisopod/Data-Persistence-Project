using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [Header("Game Objects")]
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    [Header("UI Text")]
    public Text scoreText;
    public Text bestScoreText;
    public GameObject gameOverText;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        if (m_Points < BestScoreManager.Instance.m_highestPoints)
        {
            // BestScoreManager.Instance.m_HighestPoints = m_Points;
            bestScoreText.text = $"Best Score : {BestScoreManager.Instance.m_username} : {BestScoreManager.Instance.m_highestPoints}";
        }
    }
    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void AddPoint(int point)
    {
        m_Points += point;
        scoreText.text = $"Score : {m_Points}";

        if (m_Points > BestScoreManager.Instance.m_highestPoints)
        {
            BestScoreManager.Instance.m_highestPoints = m_Points;
            bestScoreText.text = $"Best Score : {BestScoreManager.Instance.m_username} : {BestScoreManager.Instance.m_highestPoints}";
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        gameOverText.SetActive(true);

        if (m_Points > BestScoreManager.Instance.playerData.highScore)
        {
            CreateNewSave();

            string highScorePlayerName = BestScoreManager.Instance.playerData.username;
            int highScore = BestScoreManager.Instance.playerData.highScore;

            bestScoreText.text = $"Best Score : {highScorePlayerName} : {highScore}";
        }
    }

    private void CreateNewSave()
    {
        BestScoreManager.SaveData newData = new BestScoreManager.SaveData
        {
            highScore = m_Points,
            username = BestScoreManager.Instance.m_username
        };

        BestScoreManager.Instance.playerData = newData;
        BestScoreManager.Instance.SaveBestScore();
    }
}
