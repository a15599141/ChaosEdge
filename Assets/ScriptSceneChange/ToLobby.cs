﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToLobby : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        SceneManager.LoadScene("LobbyDemo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
