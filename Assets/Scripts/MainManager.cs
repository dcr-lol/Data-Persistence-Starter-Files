using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SharedState;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public static MainManager Instance;

    private void Awake()
    {
        if (Instance != null) { 
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


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

        if (SharedState.Instance?.saveData?.bestScoreName?.Length > 0 && SharedState.Instance?.saveData?.bestScore > 0) ;
        {
            Debug.Log($"Hey {SharedState.Instance?.saveData?.bestScoreName} {SharedState.Instance?.saveData?.bestScore}");
            BestScoreText.text = $"Best score: {SharedState.Instance.saveData.bestScoreName} : {SharedState.Instance.saveData.bestScore}";
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

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        // save best score here
        m_GameOver = true;
        GameOverText.SetActive(true);
        string path = Application.persistentDataPath + "/saveFile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            data.bestScore = m_Points > data.bestScore ? m_Points : data.bestScore;
            data.bestScoreName = m_Points > data.bestScore ? SharedState.Instance.playerName : data.bestScoreName;
            string save = JsonUtility.ToJson(data);
            File.WriteAllText(path, save);
        } else { 
            SaveData sd = new SaveData();
            sd.bestScore = m_Points;
            sd.bestScoreName= SharedState.Instance.playerName;
            string save = JsonUtility.ToJson(sd);
            File.WriteAllText(path, save);
        }
    }
}
