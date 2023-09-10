using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{

    public TMP_InputField register_username;
    public TextMeshProUGUI titleText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getUsername() { 
        string username = register_username.text;
        Debug.Log(username);
        //MainManager.Instance.SetUserName(username);
        titleText.text = $"Oh you're here {username}";
    }

    public void SaveUsername() {
        SharedState.Instance.SetUserName(register_username.text.Length > 0? register_username.text: "");
    }

    public void StartGame()
    {
        if (SharedState.Instance.playerName?.Length > 0)
        {
            SceneManager.LoadScene(1);
        }
    }
}
