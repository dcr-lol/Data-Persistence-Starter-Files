using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SharedState : MonoBehaviour
{
    public static SharedState Instance;
    public string playerName;
    public SaveData saveData;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadData() {
        string path = Application.persistentDataPath + "/saveFile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
    }

    public void SetUserName(string userName) { 
        playerName = userName;
    }

    public void SaveScore(int m_Points)
    {
        string path = Application.persistentDataPath + "/saveFile.json";
        if (File.Exists(path))
        {
            Debug.Log(path);
            SaveData data = saveData;
            bool newBestScore = m_Points > data.bestScore;
            data.bestScore = newBestScore ? m_Points : data.bestScore;
            data.bestScoreName = newBestScore ? SharedState.Instance.playerName : data.bestScoreName;
            Debug.Log($"{data.bestScore} {data.bestScoreName} {SharedState.Instance.playerName}");
            string save = JsonUtility.ToJson(data);
            File.WriteAllText(path, save);
        }
        else
        {
            SaveData sd = new SaveData();
            sd.bestScore = m_Points;
            sd.bestScoreName = SharedState.Instance.playerName;
            string save = JsonUtility.ToJson(sd);
            File.WriteAllText(path, save);
        }
    }

    public class SaveData
    {
        public int bestScore;
        public string bestScoreName;
    }
}
