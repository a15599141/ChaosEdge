using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SWNetwork;

public class HomeMenu : MonoBehaviour
{
    public GameObject OptionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToSingleGameSetting()
    {
        SceneManager.LoadScene("GameInitialize_SinglePlayer");
    }
    public void ToLobby()
    {
        SceneManager.LoadScene("LobbyScene");
    }
    public void ToOptions()
    {
        SceneManager.LoadScene("OptionsScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
